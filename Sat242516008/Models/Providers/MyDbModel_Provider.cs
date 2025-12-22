using MyDbModels;
using UnitOfWorks;
using Microsoft.Extensions.Logging; // Hatalarý izlemek için eklendi

namespace Providers;

public interface IMyDbModel_Provider
{
    ValueTask<IMyDbModel<TResult>> Execute<TResult>(IMyDbModel<TResult> myResultModel,
        string spName = "",
        bool isPagination = true) where TResult : class, new();

    ValueTask<IMyDbModel<TResult>> Execute<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new();

    ValueTask<IEnumerable<TResult>> GetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new();

    ValueTask<IEnumerable<TResult>> SetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new();
}

public class MyDbModel_Provider : IMyDbModel_Provider
{
    private readonly IMyDbModel_UnitOfWork _unitOfWork;

    // Constructor güncellemesi
    public MyDbModel_Provider(IMyDbModel_UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<IMyDbModel<TResult>> Execute<TResult>(IMyDbModel<TResult> myResultModel,
        string spName = "",
        bool isPagination = true) where TResult : class, new()
    {
        try
        {
            if (myResultModel == null)
                myResultModel = new MyDbModel<TResult>();

            await _unitOfWork.Execute(myResultModel, spName, isPagination);
            return myResultModel;
        }
        catch (Exception ex)
        {
            // Hata durumunda mesajý modele yazýyoruz ki UI'da görebilelim
            if (myResultModel == null) myResultModel = new MyDbModel<TResult>();
            myResultModel.Message = $"Provider Hatasý (Execute): {ex.Message}";
            return myResultModel;
        }
    }

    public async ValueTask<IMyDbModel<TResult>> Execute<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new()
    {
        var myResultModel = new MyDbModel<TResult>();
        try
        {
            if (parameters != null)
                foreach (var item in parameters)
                    myResultModel.Parameters.Params.Add(item.Key, item.Value);

            await _unitOfWork.Execute(myResultModel, spName, false);
            return myResultModel;
        }
        catch (Exception ex)
        {
            myResultModel.Message = $"Provider Hatasý (Params): {ex.Message}";
            return myResultModel;
        }
    }

    public async ValueTask<IEnumerable<TResult>> GetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new()
    {
        var result = await this.Execute<TResult>(spName, parameters);
        return result?.Items ?? Enumerable.Empty<TResult>();
    }

    public async ValueTask<IEnumerable<TResult>> SetItems<TResult>(string spName = "",
        params (string Key, object Value)[] parameters)
        where TResult : class, new()
    {
        var result = await this.Execute<TResult>(spName, parameters);
        return result?.Items ?? Enumerable.Empty<TResult>();
    }
}