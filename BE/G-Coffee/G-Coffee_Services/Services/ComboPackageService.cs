using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;

public class ComboPackageService : IComboPackageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IComboPackageRepository _comboPackageRepository;

    public ComboPackageService(IUnitOfWork unitOfWork, IComboPackageRepository comboPackageRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _comboPackageRepository = comboPackageRepository ?? throw new ArgumentNullException(nameof(comboPackageRepository));
    }

    public async Task<ComboPackage> CreateComboPackageAsync(ComboPackage comboPackage)
    {
        if (comboPackage == null)
            throw new ArgumentNullException(nameof(comboPackage));


        await _comboPackageRepository.AddAsync(comboPackage);
        await _unitOfWork.SaveChangesAsync();

        return comboPackage;
    }

    public async Task DeleteComboPackageAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ComboPackage ID is required");

        if (!Guid.TryParse(id, out Guid guidId))
            throw new ArgumentException("Invalid ComboPackage ID format");

        var comboPackage = await _comboPackageRepository.GetByIdAsync(guidId);
        if (comboPackage == null)
            throw new KeyNotFoundException($"ComboPackage with ID {id} not found");

        _comboPackageRepository.Remove(comboPackage);
        await _unitOfWork.SaveChangesAsync();
    }

    public Task DeleteComboPackageAsync(int id)
    {
        throw new NotImplementedException("Use DeleteComboPackageAsync(string id) instead.");
    }

    public async Task<IEnumerable<ComboPackage>> GetAllComboPackagesAsync()
    {
        return await _comboPackageRepository.GetAllAsync();
    }

    public async Task<ComboPackage> GetComboPackageByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ComboPackage ID is required");

        if (!Guid.TryParse(id, out Guid guidId))
            throw new ArgumentException("Invalid ComboPackage ID format");

        var comboPackage = await _comboPackageRepository.GetByIdAsync(guidId);
        if (comboPackage == null)
            throw new KeyNotFoundException($"ComboPackage with ID {id} not found");

        return comboPackage;
    }

    public async Task UpdateComboPackageAsync(ComboPackage comboPackage)
    {
        if (comboPackage == null)
            throw new ArgumentNullException(nameof(comboPackage));

        var existing = await _comboPackageRepository.GetByIdAsync(comboPackage.Id);
        if (existing == null)
            throw new KeyNotFoundException($"ComboPackage with ID {comboPackage.Id} not found");

        _comboPackageRepository.Update(comboPackage);
        await _unitOfWork.SaveChangesAsync();
    }
}
