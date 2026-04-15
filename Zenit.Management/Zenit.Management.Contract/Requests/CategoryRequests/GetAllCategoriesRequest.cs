using MediatR;

using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Requests.CategoryRequests
{
    public class GetAllCategoriesRequest : ScrollPaginationRequest, IRequest<GetAllCategoriesResponse>
    {
    }

    public class GetAllCategoriesResponse : PaginationResponse<GetAllCategoriesResponseItem>
    {
    }

    public class GetAllCategoriesResponseItem : GetDetailCategoryResponse
    {
    }
}