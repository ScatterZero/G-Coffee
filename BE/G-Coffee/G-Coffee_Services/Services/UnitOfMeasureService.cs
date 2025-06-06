using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Cofee_Repositories.Repositories;
using G_Coffee_Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfMeasureRepository _UnitOfMeasureRepository;
        private readonly IMapper _mapper;

        public UnitOfMeasureService(IUnitOfWork unitOfWork, IUnitOfMeasureRepository UnitOfMeasureRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _UnitOfMeasureRepository = UnitOfMeasureRepository ?? throw new ArgumentNullException(nameof(UnitOfMeasureRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UnitOfMeasureDTO> CreateUnitOfMeasureAsync(UnitOfMeasureDTO UnitOfMeasure)
        {
            if (UnitOfMeasure == null) throw new ArgumentNullException(nameof(UnitOfMeasure));

            try
            {
                // Correctly map UnitOfMeasureDTO to UnitOfMeasure before passing to AddAsync
                var UnitOfMeasureEntity = _mapper.Map<UnitsOfMeasure>(UnitOfMeasure);
                UnitOfMeasureEntity.CreatedDate = DateTime.UtcNow;
                UnitOfMeasureEntity.UpdatedDate = DateTime.UtcNow;

                await _UnitOfMeasureRepository.AddAsync(UnitOfMeasureEntity); // Fix: Pass the mapped UnitOfMeasure entity
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<UnitOfMeasureDTO>(UnitOfMeasureEntity); // Fix: Return the mapped UnitOfMeasureDTO
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating UnitOfMeasure", ex);
            }
        }

        public async Task DeleteUnitOfMeasureAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("UnitOfMeasure ID is required");

            try
            {
                var UnitOfMeasure = await _UnitOfMeasureRepository.GetByIdAsync(id);
                if (UnitOfMeasure == null) throw new Exception($"UnitOfMeasure with ID {id} not found");

                _UnitOfMeasureRepository.Remove(UnitOfMeasure);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting UnitOfMeasure with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<UnitOfMeasureDTO>> GetAllUnitOfMeasuresAsync()
        {
            try
            {
                var UnitOfMeasures = await _UnitOfMeasureRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<UnitOfMeasureDTO>>(UnitOfMeasures);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all UnitOfMeasures", ex);
            }
        }

        public async Task<UnitOfMeasureDTO> GetUnitOfMeasureByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("UnitOfMeasure ID is required");

            try
            {
                var UnitOfMeasure = await _UnitOfMeasureRepository.GetByIdAsync(id);
                if (UnitOfMeasure == null) throw new Exception($"UnitOfMeasure with ID {id} not found");

                return _mapper.Map<UnitOfMeasureDTO>(UnitOfMeasure);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving UnitOfMeasure with ID {id}", ex);
            }
        }

        public async Task UpdateUnitOfMeasureAsync(UnitOfMeasureDTO UnitOfMeasureDto)
        {
            if (UnitOfMeasureDto == null) throw new ArgumentNullException(nameof(UnitOfMeasureDto));

            try
            {
                var UnitOfMeasure = await _UnitOfMeasureRepository.GetByIdAsync(UnitOfMeasureDto.UnitOfMeasureId);
                if (UnitOfMeasure == null) throw new Exception($"UnitOfMeasure with ID {UnitOfMeasureDto.UnitOfMeasureId} not found");

                _mapper.Map(UnitOfMeasureDto, UnitOfMeasure);
                _UnitOfMeasureRepository.Update(UnitOfMeasure); // Removed 'await' since Update is a void method.
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating UnitOfMeasure with ID {UnitOfMeasureDto.UnitOfMeasureId}", ex);
            }
        }
    }
}
