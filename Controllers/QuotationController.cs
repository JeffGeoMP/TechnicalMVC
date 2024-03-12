using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Transactions;
using TechnicalMVC.Data;
using TechnicalMVC.Models.Entities;
using TechnicalMVC.Models.Quotation;

namespace TechnicalMVC.Controllers
{
    public class QuotationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuotationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListQuotation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddQuotation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditQuotation(int quotationId)
        {
            var quotation = await  _context.Quotations.Where(x => x.Id == quotationId).Select(
                x => new QuotationViewModel
                {
                    Id = x.Id,
                    Serie = x.Serie,
                    Client = new ClientViewModel
                    {
                        Id = x.Client.Id,
                        Name = x.Client.Name,
                        DPI = x.Client.DPI,
                        NIT = x.Client.NIT,
                        Address = x.Client.Address
                    },
                    Total = Math.Round(x.Total, 2),
                    CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy")
                }).FirstOrDefaultAsync();

            var products = await _context.QuotationDetails.Where(x => x.QuotationId == quotationId)
                .Include(x => x.Product)
                .Select(x => new QuotationDetailViewModel
                {
                    Id = x.Product.Id,
                    SKU = x.Product.SKU,
                    Description = x.Product.Description,
                    Qty = x.Qty,
                    Price = Math.Round(x.Product.Price, 2),
                    Subtotal = Math.Round(x.Subtotal, 2)
                }).ToListAsync();

            quotation.Detail = products;

            return View(quotation);
        }

        /// <summary>
        /// Get for all quotations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllQuotation()
        {
            var quotations = await _context.Quotations.Include(c => c.Client)
                .Select(x => new QuotationViewModel
                {
                    Id = x.Id,
                    Serie = x.Serie,
                    Client = new ClientViewModel
                    {
                        Id = x.Client.Id,
                        Name = x.Client.Name,
                        DPI = x.Client.DPI,
                        NIT = x.Client.NIT,
                        Address = x.Client.Address
                    },
                    Total = x.Total,
                    CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy")
                }).ToListAsync();

            return Ok(quotations);
        }

        /// <summary>
        /// Get quotation by id.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDetailQuotation(int productId, int qty)
        {
            var productDetail = await _context.Products.Where(x => x.Id == productId)
                .Select(x => new QuotationDetailViewModel()
                {
                    Id = x.Id,
                    SKU = x.SKU,
                    Description = x.Description,
                    Qty = qty,
                    Price = Math.Round(x.Price, 2),
                    Subtotal = Math.Round(qty * x.Price, 2)

                }).FirstOrDefaultAsync();

            return Ok(productDetail);
        }

        /// <summary>
        /// Add new quotation.
        /// </summary>
        /// <param name="quotation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddQuotation([FromBody] QuotationViewModel quotation)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var dataInsert = await _context.Quotations.AddAsync(new Quotation
                    {
                        Serie = quotation.Serie,
                        ClientId = quotation.Client.Id,
                        Total = quotation.Total,
                    });

                    await _context.SaveChangesAsync();

                    foreach (var detail in quotation.Detail)
                    {
                        await _context.QuotationDetails.AddAsync(new QuotationDetail
                        {
                            QuotationId = dataInsert.Entity.Id,
                            ProductId = detail.Id,
                            Qty = detail.Qty,
                            Subtotal = detail.Subtotal
                        });
                    }

                    await _context.SaveChangesAsync();

                    scope.Complete();
                    return Ok();
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return BadRequest(e.Message);
                }
            }
        }

        /// <summary>
        /// Edit quotation.
        /// </summary>
        /// <param name="quotation"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditQuotation([FromBody] QuotationViewModel quotation)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var dataUpdate = await _context.Quotations.FindAsync(quotation.Id);
                    dataUpdate.Serie = quotation.Serie;
                    dataUpdate.ClientId = quotation.Client.Id;
                    dataUpdate.Total = quotation.Total;

                    _context.Quotations.Update(dataUpdate);
                    await _context.SaveChangesAsync();

                    var details = await _context.QuotationDetails.Where(x => x.QuotationId == quotation.Id).ToListAsync();
                    _context.QuotationDetails.RemoveRange(details);
                    await _context.SaveChangesAsync();

                    foreach (var detail in quotation.Detail)
                    {
                        await _context.QuotationDetails.AddAsync(new QuotationDetail
                        {
                            QuotationId = quotation.Id,
                            ProductId = detail.Id,
                            Qty = detail.Qty,
                            Subtotal = detail.Subtotal
                        });
                    }

                    await _context.SaveChangesAsync();

                    scope.Complete();
                    return Ok();
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return BadRequest(e.Message);
                }
            }
        }

        /// <summary>
        /// Delete quotation.
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RemoveQuotation(int quotationId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var detail = await _context.QuotationDetails.Where(x => x.QuotationId == quotationId).ToListAsync();
                    _context.QuotationDetails.RemoveRange(detail);
                    await _context.SaveChangesAsync();

                    var quotation = await _context.Quotations.FindAsync(quotationId);
                    _context.Quotations.Remove(quotation);
                    await _context.SaveChangesAsync();

                    scope.Complete();
                    return Ok();
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return BadRequest(e.Message);
                }
            }
        }
    }

    
}
