using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;

public class PayOSService : IPayOSService
{
    private readonly PayOS _payOS;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentRepository _paymentRepository;

    public PayOSService(IConfiguration config,IMapper mapper, IUnitOfWork unitOfWork, IPaymentRepository paymentRepository )
    {
        _config = config;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _payOS = new PayOS(
            _config["PayOS:ClientId"],
            _config["PayOS:ApiKey"],
            _config["PayOS:ChecksumKey"]
        );
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
    }

