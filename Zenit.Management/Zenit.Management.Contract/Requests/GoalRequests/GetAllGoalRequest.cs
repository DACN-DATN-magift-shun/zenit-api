using MediatR;

using Zenit.Management.Common.Enums;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Requests.GoalRequests
{
    public class GetAllGoalRequest : ScrollPaginationRequest, IRequest<GetAllGoalResponse>
    {
    }

    public class GetAllGoalResponse : PaginationResponse<GetAllGoalResponseItem>
    {
    }

    public class GetAllGoalResponseItem : GetDetailGoalResponse
    {
    }
}