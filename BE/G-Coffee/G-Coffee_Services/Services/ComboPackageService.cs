using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class ComboPackageService : IComboPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IComboPackageRepository _ComboPackageRepository;

        public ComboPackageService(IUnitOfWork unitOfWork, IComboPackageRepository ComboPackageRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _ComboPackageRepository = ComboPackageRepository ?? throw new ArgumentNullException(nameof(ComboPackageRepository));
        }
        public async Task<ComboPackage> CreateComboPackageAsync(ComboPackage ComboPackage)
        {
            if (ComboPackage == null) throw new ArgumentNullException(nameof(ComboPackage));

            try
            {
                // Correctly map ComboPackageDTO to ComboPackage before passing to AddAsync
                // Assuming ComboPackageEntity is the mapped entity
                var ComboPackageEntity = ComboPackage; // Replace with actual mapping logic if needed

                await _ComboPackageRepository.AddAsync(ComboPackageEntity); // Fix: Pass the mapped ComboPackage entity
                await _unitOfWork.SaveChangesAsync();

                return ComboPackageEntity; // Ensure a value is returned
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating ComboPackage", ex);
            }
        }

        public async Task DeleteComboPackageAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ComboPackage ID is required");

            try
            {
                var ComboPackage = await _ComboPackageRepository.GetByIdAsync(Guid.Parse(id));
                if (ComboPackage == null) throw new Exception($"ComboPackage with ID {id} not found");

                _ComboPackageRepository.Remove(ComboPackage);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting ComboPackage with ID {id}", ex);
            }
        }

        public Task DeleteComboPackageAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ComboPackage>> GetAllComboPackagesAsync()
        {
            try
            {
                var ComboPackages = await _ComboPackageRepository.GetAllAsync();
                return ComboPackages; // Ensure a value is returned
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all ComboPackages", ex);
            }
        }

        public async Task<ComboPackage> GetComboPackageByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ComboPackage ID is required");

            try
            {
                var ComboPackage = await _ComboPackageRepository.GetByIdAsync(Guid.Parse(id));
                if (ComboPackage == null) throw new Exception($"ComboPackage with ID {id} not found");

                return ComboPackage; // Ensure a value is returned
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving ComboPackage with ID {id}", ex);
            }
        }

       

        public async Task UpdateComboPackageAsync(ComboPackage comboPackage)
        {
            if (comboPackage == null) throw new ArgumentNullException(nameof(ComboPackage));

            try
            {
                var ComboPackage = await _ComboPackageRepository.GetByIdAsync(comboPackage.Id);
                if (ComboPackage == null) throw new Exception($"ComboPackage with ID {comboPackage.Id} not found");

                _ComboPackageRepository.Update(ComboPackage); // Removed 'await' since Update is a void method.
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating ComboPackage with ID {comboPackage.Id}", ex);
            }
        }
    }
}
