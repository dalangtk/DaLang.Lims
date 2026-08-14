namespace DaLang.Lims.Agent.Contracts.Dto;

public class UserQueryResponse
{
    public int code { get; set; }
    public Result result { get; set; }
}

public class Result
{
    public string answer { get; set; }
}
