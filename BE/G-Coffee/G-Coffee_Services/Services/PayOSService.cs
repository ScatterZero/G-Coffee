using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
using G_Coffee_Services.IServices;
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
    private readonly static string _checksumKey = Environment.GetEnvironmentVariable("PAYOS_CHECKSUM")
    ?? throw new InvalidOperationException("PAYOS_CHECKSUM environment variable is not set.");

    public PayOSService(IConfiguration config,IMapper mapper, IUnitOfWork unitOfWork, IPaymentRepository paymentRepository, PayOS payOS)
    {
        _config = config;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _payOS = payOS;

    }

    public async Task<PaymentResponse> CreatePaymentLink(PaymentRequest request)
    {
        request.CancelUrl ??= _config["PayOS:CancelUrl"];
        request.ReturnUrl ??= _config["PayOS:ReturnUrl"];

        // ✅ Rút gọn description tối đa 25 ký tự
        var desc = request.Description ?? $"DH {request.OrderCode}";
        if (desc.Length > 25)
        {
            desc = desc.Substring(0, 25);
        }

        var payOSRequest = new PaymentData(
            orderCode: request.OrderCode,
            amount: request.Amount,
            description: desc, // sử dụng desc đã rút gọn
            items: new List<ItemData>(), // Default empty list for 'items'
            cancelUrl: request.CancelUrl,
            returnUrl: request.ReturnUrl
        );

        var paymentLinkResponse = await _payOS.createPaymentLink(payOSRequest);

        return new PaymentResponse
        {
            CheckoutUrl = paymentLinkResponse.checkoutUrl,
            OrderCode = paymentLinkResponse.orderCode,
            Amount = request.Amount,
            Status = "PENDING"
        };
    }

    public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO dto)
    {
        if (dto == null)
            throw new ArgumentException("Payment data cannot be null");

        // Map PaymentDTO to Payment entity instead of UnitsOfMeasure
        var entity = _mapper.Map<Payment>(dto);

        await _paymentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PaymentDTO>(entity);
    }

        public async Task DeletePaymentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Unit of Measure ID is required");

            var unit = await _paymentRepository.GetByIdAsync(id);
            if (unit == null)
                throw new KeyNotFoundException($"Unit of Measure with ID {id} not found");

        _paymentRepository.Remove(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            var entities = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDTO>>(entities);
        }

        public async Task<PaymentDTO> GetPaymentByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Unit of Measure ID is required");

            var unit = await _paymentRepository.GetByIdAsync(id);
            if (unit == null)
                throw new KeyNotFoundException($"Unit of Measure with ID {id} not found");

            return _mapper.Map<PaymentDTO>(unit);
        }

        public async Task UpdatePaymentAsync(PaymentDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Unit of Measure data cannot be null");

            var existing = await _paymentRepository.GetByIdAsync(dto.PaymentId);
            if (existing == null)
                throw new KeyNotFoundException($"Unit of Measure with ID {dto.PaymentId} not found");

            _mapper.Map(dto, existing);
        _paymentRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    //public async Task<string> CheckOut(Guid orderId)
    //{
    //    var order = await _orderRepository.GetByPropertyAsync(o => o.Id == orderId, includeProperties: "Cart, Cart.CartItems, Cart.CartItems.Product", tracked: false)
    //         ?? throw new KeyNotFoundException("Order not found!");

    //    if (order.Status != OrderStatus.Created)
    //    {
    //        throw new InvalidOperationException("Order can only be checked out if it is in the Created status.");
    //    }


    //    var user = await _userRepository.GetByIdAsync(order.UserId)
    //        ?? throw new KeyNotFoundException("User not found!");

    //    var returnUrl = @$"http://localhost:3000/paymnet-success";
    //    var cancelUrl = @$"http://localhost:3000/paymnet-fail";

    //    int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));


    //    Payment transaction = new()
    //    {
    //        OrderId = order.Id,
    //        OrderCode = orderCode,
    //    };

    //    var request = new PaymentData(
    //        orderCode: orderCode,
    //        amount: (int)(order.Amount),
    //        description: $"Đơn hàng #{orderCode}",
    //        returnUrl: returnUrl,
    //        cancelUrl: cancelUrl,
    //        buyerName: $"{user.FullName}",
    //        buyerPhone: user.PhoneNumber,
    //        items: items.ToList()
    //    );

    //    await _paymentRepository.AddAsync(transaction);
    //    await _unitOfWork.SaveChangesAsync();
    //    var paymentLinkResp = await _payOS.createPaymentLink(request);

    //    return paymentLinkResp.checkoutUrl;
    //}

    //public async Task<bool> HandleWebhook(PayOSWebhookRequest payload)
    //{
    //    using var transaction = await _unitOfWork.BeginTransactionAsync();
    //    try
    //    {
    //        var orderCode = payload.Data.OrderCode;

    //        var payment = await _paymentRepository.GetByIdAsync(p => p.OrderCode == orderCode, tracked: true)
    //            ?? throw new KeyNotFoundException("Payment not found!");

    //        _mapper.Map(payload, payment);
    //        payment.UpdatedDate = DateTime.UtcNow;
    //        _paymentRepository.Update(payment);

    //        var order = await _orderRepository.GetByIdAsync(o => o.Id == payment.OrderId: tracked: true)
    //            ?? throw new KeyNotFoundException("Order not found!");

    //        var cart = order.Cart;
    //        if (payment.Success)
    //        {
    //            cart.IsOrdered = true;
    //            cart.UpdatedDate = DateTime.UtcNow;
    //            foreach (var item in cart.CartItems)
    //            {
    //                var product = await _productRepository.GetByIdAsync(item.ProductId)
    //                    ?? throw new KeyNotFoundException($"Product with ID {item.ProductId} not found!");

    //                product.Stock -= item.Quantity;
    //                if (product.Stock < 0)
    //                {
    //                    await transaction.RollbackAsync();
    //                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");
    //                }
    //                _productRepository.Update(product);
    //            }
    //            order.Status = OrderStatus.Paid;
    //        }
    //        else
    //        {
    //            order.Status = OrderStatus.Failed;
    //        }

    //        order.UpdatedDate = DateTime.UtcNow;
    //        cart.IsOrdered = true;
    //        await _orderRepository.Update(order);
    //        await _unitOfWork.SaveChangesAsync();
    //        await transaction.CommitAsync();

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        await transaction.RollbackAsync();
    //        throw new Exception(ex.Message);
    //    }
    //}

    public bool IsValidData(string transaction, string transactionSignature)
    {
        try
        {
            JObject jsonObject = JObject.Parse(transaction);

            var sortedKeys = jsonObject.Properties()
                                       .Select(p => p.Name)
                                       .OrderBy(k => k, StringComparer.Ordinal)
                                       .ToList();

            var sb = new StringBuilder();
            for (int i = 0; i < sortedKeys.Count; i++)
            {
                var key = sortedKeys[i];
                var value = jsonObject[key]?.ToString();
                sb.Append($"{key}={value}");
                if (i < sortedKeys.Count - 1)
                    sb.Append("&");
            }

            string computedSignature = ComputeHmacSHA256(sb.ToString(), _checksumKey);
            return computedSignature.Equals(transactionSignature, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }

    private string ComputeHmacSHA256(string message, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            byte[] hash = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}

