namespace Qincai.Api.Dtos
{
    public interface ISortedParam
    {
        string OrderBy { get; set; }
        bool Descending { get; set; }
    }
}
