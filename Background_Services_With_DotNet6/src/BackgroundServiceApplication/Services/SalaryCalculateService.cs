using BackgroundServiceApplication.Services.Contracts;
using BackgroundServiceApplication.Wrappers;
using BackgroundServiceApplication.Wrappers.Contracts;

namespace BackgroundServiceApplication.Services;

public class SalaryCalculateService : ISalaryCalculateService
{
    #region Fields
    private readonly ISalaryCalculateWrapper _salaryCalculateWrapper;
    #endregion Fields

    #region Ctor

    public SalaryCalculateService(ISalaryCalculateWrapper salaryCalculateWrapper)
    {
        this._salaryCalculateWrapper = salaryCalculateWrapper;
    }

    #endregion Ctor

    #region Methods

    public async Task SalaryCalculateAsync()
    {
        await _salaryCalculateWrapper.SalaryCalculateAsync();
    }

    #endregion Methods
}
