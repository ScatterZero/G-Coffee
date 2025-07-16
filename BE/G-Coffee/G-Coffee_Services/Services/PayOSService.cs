using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
using G_Coffee_Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

public class PayOSService : IPayOSService
{
    private readonly PayOS _payOS;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Order> _orderRepository;
    private readonly GcoffeeDbContext _context;


    public PayOSService(IConfiguration config, IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<User> userRepository, IGenericRepository<Product> productRepository, PayOS payOS, IGenericRepository<Order> orderRepository, IPaymentRepository paymentRepository, GcoffeeDbContext context)
    {
        _config = config;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _payOS = payOS;
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _context = context;


    }
    public async Task<object> CreatePaymentLink(Guid orderID)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var orderRepo = new GenericRepository<Order>(_context);
            var order = await orderRepo.GetByIdAsync(orderID);

            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            var description = "Thanh toán đơn hàng" + " " + order.OrderCode;
            long orderCode = long.Parse(DateTimeOffset.Now.ToString("ffffff"));

            var baseUrl = "http://localhost:3000";
            //var baseUrl = "https://exe-201-home.vercel.app";
            var returnSuccessUrl = $"{baseUrl}/success.html?orderCode={orderCode}";
            var returnCancelledUrl = $"{baseUrl}/cancel.html?orderCode={orderCode}";

            List<ItemData> emptyItems = new List<ItemData>();

            var paymentData = new PaymentData(
                orderCode,
                order.Amount,
                description,
                emptyItems,
                returnCancelledUrl,
                returnSuccessUrl
            );

            var createPayment = await _payOS.createPaymentLink(paymentData);

            // Cập nhật OrderCode
            order.OrderCode = orderCode;
            await orderRepo.UpdateAsync(order);

            await transaction.CommitAsync();

            return new
            {
                checkout = createPayment.checkoutUrl
            };
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    //public async Task<PaymentResponse> CreatePaymentLink(PaymentRequest request)
    //{
    //    request.CancelUrl ??= _config["PayOS:CancelUrl"];
    //    request.ReturnUrl ??= _config["PayOS:ReturnUrl"];

    //    // ✅ Rút gọn description tối đa 25 ký tự
    //    var desc = request.Description ?? $"DH {request.OrderCode}";
    //    if (desc.Length > 25)
    //    {
    //        desc = desc.Substring(0, 25);
    //    }

    //    var payOSRequest = new PaymentData(
    //        orderCode: request.OrderCode,
    //        amount: request.Amount,
    //        description: desc, // sử dụng desc đã rút gọn
    //        items: new List<ItemData>(), // Default empty list for 'items'
    //        cancelUrl: request.CancelUrl,
    //        returnUrl: request.ReturnUrl
    //    );

    //    var paymentLinkResponse = await _payOS.createPaymentLink(payOSRequest);

    //    return new PaymentResponse
    //    {
    //        CheckoutUrl = paymentLinkResponse.checkoutUrl,
    //        OrderCode = paymentLinkResponse.orderCode,
    //        Amount = request.Amount,
    //        Status = "PENDING"
    //    };
    //}

    //public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO dto)
    //{
    //    if (dto == null)
    //        throw new ArgumentException("Payment data cannot be null");

    //    // Map PaymentDTO to Payment entity instead of UnitsOfMeasure
    //    var entity = _mapper.Map<Payment>(dto);

    //    await _paymentRepository.AddAsync(entity);
    //    await _unitOfWork.SaveChangesAsync();

    //    return _mapper.Map<PaymentDTO>(entity);
    //}

    //public async Task DeletePaymentAsync(string id)
    //{
    //    if (string.IsNullOrWhiteSpace(id))
    //        throw new ArgumentException("Unit of Measure ID is required");

    //    var unit = await _paymentRepository.GetByIdAsync(id);
    //    if (unit == null)
    //        throw new KeyNotFoundException($"Unit of Measure with ID {id} not found");

    //    _paymentRepository.Remove(unit);
    //    await _unitOfWork.SaveChangesAsync();
    //}

    //public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
    //{
    //    var entities = await _paymentRepository.GetAllAsync();
    //    return _mapper.Map<IEnumerable<PaymentDTO>>(entities);
    //}

    //public async Task<PaymentDTO> GetPaymentByIdAsync(string id)
    //{
    //    if (string.IsNullOrWhiteSpace(id))
    //        throw new ArgumentException("Unit of Measure ID is required");

    //    var unit = await _paymentRepository.GetByIdAsync(id);
    //    if (unit == null)
    //        throw new KeyNotFoundException($"Unit of Measure with ID {id} not found");

    //    return _mapper.Map<PaymentDTO>(unit);
    //}

    //public async Task UpdatePaymentAsync(PaymentDTO dto)
    //{
    //    if (dto == null)
    //        throw new ArgumentException("Unit of Measure data cannot be null");

    //    var existing = await _paymentRepository.GetByIdAsync(dto.PaymentId);
    //    if (existing == null)
    //        throw new KeyNotFoundException($"Unit of Measure with ID {dto.PaymentId} not found");

    //    _mapper.Map(dto, existing);
    //    _paymentRepository.Update(existing);
    //    await _unitOfWork.SaveChangesAsync();
    //}


    //public bool IsValidData(string transaction, string transactionSignature)
    //{
    //    try
    //    {
    //        JObject jsonObject = JObject.Parse(transaction);

    //        var sortedKeys = jsonObject.Properties()
    //                                   .Select(p => p.Name)
    //                                   .OrderBy(k => k, StringComparer.Ordinal)
    //                                   .ToList();

    //        var sb = new StringBuilder();
    //        for (int i = 0; i < sortedKeys.Count; i++)
    //        {
    //            var key = sortedKeys[i];
    //            var value = jsonObject[key]?.ToString();
    //            sb.Append($"{key}={value}");
    //            if (i < sortedKeys.Count - 1)
    //                sb.Append("&");
    //        }

    //        string computedSignature = ComputeHmacSHA256(sb.ToString(), _checksumKey);
    //        return computedSignature.Equals(transactionSignature, StringComparison.OrdinalIgnoreCase);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("Error: " + ex.Message);
    //        return false;
    //    }
    //}

    //private string ComputeHmacSHA256(string message, string key)
    //{
    //    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
    //    byte[] messageBytes = Encoding.UTF8.GetBytes(message);

    //    using (var hmac = new HMACSHA256(keyBytes))
    //    {
    //        byte[] hash = hmac.ComputeHash(messageBytes);
    //        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    //    }
    //}

    public async Task HandlePaymentWebhook(WebhookType webhookData)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            WebhookData data = _payOS.verifyPaymentWebhookData(webhookData);
            var orderRepo = new GenericRepository<Order>(_context);

            var order = await orderRepo.GetFirstOrDefaultAsync(x => x.OrderCode == data.orderCode);

            if (order != null)
            {
                // Chỉ cập nhật nếu thanh toán thành công
                if (data.code == "00")
                {
                    order.Status = "PAID";
                    order.CreatedAt = DateTime.Now;
                    // Có thể cập nhật thêm hạn sử dụng nếu cần
                    // order.ExpiryDate = DateTime.Now.AddDays(package.Duration);


                    await orderRepo.UpdateAsync(order);
                }

                // Nếu thất bại hoặc hủy, không làm gì cả
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

