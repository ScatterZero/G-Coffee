using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class TransactionDetailService : ITransactionDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionDetailService> _logger;

        public TransactionDetailService(
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            ITransactionDetailRepository transactionDetailRepository,
            IMapper mapper,
            ILogger<TransactionDetailService> logger)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _transactionDetailRepository = transactionDetailRepository ?? throw new ArgumentNullException(nameof(transactionDetailRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TransactionDetailDTO> CreateTransactionDetailAsync(TransactionDetailDTO transactionDetailDto)
        {
            if (transactionDetailDto == null)
            {
                _logger.LogError("TransactionDetailDTO là null");
                throw new ArgumentNullException(nameof(transactionDetailDto));
            }
            if (transactionDetailDto.TransactionId == Guid.Empty)
            {
                _logger.LogError("TransactionId không hợp lệ: Guid.Empty");
                throw new ArgumentException("TransactionId không hợp lệ. Phải là một GUID hợp lệ.", nameof(transactionDetailDto.TransactionId));
            }

            try
            {
                _logger.LogInformation("Tạo TransactionDetail với TransactionId: {TransactionId}", transactionDetailDto.TransactionId);

                var transactionExists = await _transactionRepository.AnyAsync(t => t.TransactionId == transactionDetailDto.TransactionId);
                if (!transactionExists)
                {
                    _logger.LogError("Không tìm thấy Transaction với TransactionId: {TransactionId}", transactionDetailDto.TransactionId);
                    throw new InvalidOperationException($"Không tìm thấy Transaction với ID {transactionDetailDto.TransactionId}.");
                }

                var transactionDetailEntity = _mapper.Map<TransactionDetail>(transactionDetailDto);
                _logger.LogInformation("TransactionId sau ánh xạ: {TransactionId}", transactionDetailEntity.TransactionId);

                transactionDetailEntity.CreatedDate = DateTime.UtcNow;
                transactionDetailEntity.UpdatedDate = DateTime.UtcNow;

                await _transactionDetailRepository.AddAsync(transactionDetailEntity);
                await _unitOfWork.SaveChangesAsync();

                var resultDto = _mapper.Map<TransactionDetailDTO>(transactionDetailEntity);
                _logger.LogInformation("Đã tạo TransactionDetail thành công với TransactionDetailId: {TransactionDetailId}", resultDto.TransactionDetailId);
                return resultDto;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Lỗi cơ sở dữ liệu khi tạo TransactionDetail với TransactionId: {TransactionId}", transactionDetailDto.TransactionId);
                throw new Exception($"Lỗi khi tạo TransactionDetail do lỗi cơ sở dữ liệu: {ex.InnerException?.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi tạo TransactionDetail với TransactionId: {TransactionId}", transactionDetailDto.TransactionId);
                throw new Exception($"Lỗi khi tạo TransactionDetail: {ex.Message}", ex);
            }
        }

        public async Task DeleteTransactionDetailAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("TransactionDetail ID là null hoặc rỗng");
                throw new ArgumentException("Yêu cầu TransactionDetail ID", nameof(id));
            }

            if (!Guid.TryParse(id, out Guid guidId))
            {
                _logger.LogError("TransactionDetail ID không phải là Guid hợp lệ: {Id}", id);
                throw new ArgumentException($"TransactionDetail ID không phải là Guid hợp lệ: {id}", nameof(id));
            }

            try
            {
                _logger.LogInformation("Xóa TransactionDetail với ID: {Id}", guidId);
                var transactionDetail = await _transactionDetailRepository.GetByIdAsync(guidId);
                if (transactionDetail == null)
                {
                    _logger.LogWarning("Không tìm thấy TransactionDetail với ID: {Id}", guidId);
                    throw new KeyNotFoundException($"Không tìm thấy TransactionDetail với ID {guidId}");
                }

                _transactionDetailRepository.Remove(transactionDetail);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Đã xóa TransactionDetail thành công với ID: {Id}", guidId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa TransactionDetail với ID: {Id}", guidId);
                throw new Exception($"Lỗi khi xóa TransactionDetail với ID {guidId}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<TransactionDetailDTO>> GetAllTransactionDetailsAsync()
        {
            try
            {
                _logger.LogInformation("Lấy tất cả TransactionDetails");
                var transactionDetails = await _transactionDetailRepository.GetAllAsync();
                var result = _mapper.Map<IEnumerable<TransactionDetailDTO>>(transactionDetails);
                _logger.LogInformation("Đã lấy {Count} TransactionDetails", result.Count());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả TransactionDetails");
                throw new Exception($"Lỗi khi lấy tất cả TransactionDetails: {ex.Message}", ex);
            }
        }

        public async Task<TransactionDetailDTO> GetTransactionDetailByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("TransactionDetail ID là null hoặc rỗng");
                throw new ArgumentException("Yêu cầu TransactionDetail ID", nameof(id));
            }

            if (!Guid.TryParse(id, out Guid guidId))
            {
                _logger.LogError("TransactionDetail ID không phải là Guid hợp lệ: {Id}", id);
                throw new ArgumentException($"TransactionDetail ID không phải là Guid hợp lệ: {id}", nameof(id));
            }

            try
            {
                _logger.LogInformation("Lấy TransactionDetail với ID: {Id}", guidId);
                var transactionDetail = await _transactionDetailRepository.GetByIdAsync(guidId);
                if (transactionDetail == null)
                {
                    _logger.LogWarning("Không tìm thấy TransactionDetail với ID: {Id}", guidId);
                    throw new KeyNotFoundException($"Không tìm thấy TransactionDetail với ID {guidId}");
                }

                var result = _mapper.Map<TransactionDetailDTO>(transactionDetail);
                _logger.LogInformation("Đã lấy TransactionDetail thành công với ID: {Id}", guidId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy TransactionDetail với ID: {Id}", guidId);
                throw new Exception($"Lỗi khi lấy TransactionDetail với ID {guidId}: {ex.Message}", ex);
            }
        }

        public async Task UpdateTransactionDetailAsync(TransactionDetailDTO transactionDetailDto)
        {
            if (transactionDetailDto == null)
            {
                _logger.LogError("TransactionDetailDTO là null");
                throw new ArgumentNullException(nameof(transactionDetailDto));
            }
            if (transactionDetailDto.TransactionDetailId == Guid.Empty)
            {
                _logger.LogError("TransactionDetailId không hợp lệ: Guid.Empty");
                throw new ArgumentException("Yêu cầu TransactionDetail ID hợp lệ", nameof(transactionDetailDto.TransactionDetailId));
            }
            if (transactionDetailDto.TransactionId == Guid.Empty)
            {
                _logger.LogError("TransactionId không hợp lệ: Guid.Empty");
                throw new ArgumentException("TransactionId không hợp lệ. Phải là một GUID hợp lệ.", nameof(transactionDetailDto.TransactionId));
            }

            try
            {
                _logger.LogInformation("Cập nhật TransactionDetail với TransactionDetailId: {Id}, TransactionId: {TransactionId}",
                    transactionDetailDto.TransactionDetailId, transactionDetailDto.TransactionId);

                var transactionExists = await _transactionRepository.AnyAsync(t => t.TransactionId == transactionDetailDto.TransactionId);
                if (!transactionExists)
                {
                    _logger.LogError("Không tìm thấy Transaction với TransactionId: {TransactionId}", transactionDetailDto.TransactionId);
                    throw new InvalidOperationException($"Không tìm thấy Transaction với ID {transactionDetailDto.TransactionId}.");
                }

                var transactionDetail = await _transactionDetailRepository.GetByIdAsync(transactionDetailDto.TransactionDetailId);
                if (transactionDetail == null)
                {
                    _logger.LogWarning("Không tìm thấy TransactionDetail với ID: {Id}", transactionDetailDto.TransactionDetailId);
                    throw new KeyNotFoundException($"Không tìm thấy TransactionDetail với ID {transactionDetailDto.TransactionDetailId}");
                }

                _mapper.Map(transactionDetailDto, transactionDetail);
                transactionDetail.UpdatedDate = DateTime.UtcNow;

                _transactionDetailRepository.Update(transactionDetail);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Đã cập nhật TransactionDetail thành công với ID: {Id}", transactionDetailDto.TransactionDetailId);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Lỗi cơ sở dữ liệu khi cập nhật TransactionDetail với ID: {Id}", transactionDetailDto.TransactionDetailId);
                throw new Exception($"Lỗi khi cập nhật TransactionDetail do lỗi cơ sở dữ liệu: {ex.InnerException?.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi cập nhật TransactionDetail với ID: {Id}", transactionDetailDto.TransactionDetailId);
                throw new Exception($"Lỗi khi cập nhật TransactionDetail với ID {transactionDetailDto.TransactionDetailId}: {ex.Message}", ex);
            }
        }
    }
}
