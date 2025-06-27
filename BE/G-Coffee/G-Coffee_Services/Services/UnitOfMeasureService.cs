using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;
        private readonly IMapper _mapper;

        public UnitOfMeasureService(IUnitOfWork unitOfWork, IUnitOfMeasureRepository unitOfMeasureRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _unitOfMeasureRepository = unitOfMeasureRepository ?? throw new ArgumentNullException(nameof(unitOfMeasureRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UnitOfMeasureDTO> CreateUnitOfMeasureAsync(UnitOfMeasureDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Unit of Measure data cannot be null");

            var entity = _mapper.Map<UnitsOfMeasure>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;

            await _unitOfMeasureRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UnitOfMeasureDTO>(entity);
        }

        public async Task DeleteUnitOfMeasureAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Unit of Measure ID is required");

            var unit = await _unitOfMeasureRepository.GetByIdAsync(id);
            if (unit == null)
                throw new KeyNotFoundException($"Unit of Measure with ID {id} not found");

            _unitOfMeasureRepository.Remove(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UnitOfMeasureDTO>> GetAllUnitOfMeasuresAsync()
        {
            var entities = await _unitOfMeasureRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UnitOfMeasureDTO>>(entities);
        }

        public async Task<UnitOfMeasureDTO> GetUnitOfMeasureByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Unit of Measure ID is required");

            var unit = await _unitOfMeasureRepository.GetByIdAsync(id);
            if (unit == null)
                throw new KeyNotFoundException($"Unit of Measure with ID {id} not found");

            return _mapper.Map<UnitOfMeasureDTO>(unit);
        }

        public async Task UpdateUnitOfMeasureAsync(UnitOfMeasureDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Unit of Measure data cannot be null");

            var existing = await _unitOfMeasureRepository.GetByIdAsync(dto.UnitOfMeasureId);
            if (existing == null)
                throw new KeyNotFoundException($"Unit of Measure with ID {dto.UnitOfMeasureId} not found");

            _mapper.Map(dto, existing);
            _unitOfMeasureRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
