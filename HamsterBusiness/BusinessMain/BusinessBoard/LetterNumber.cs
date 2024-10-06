using HamsterBusiness.BusinessMain.BusinessUtil;

namespace HamsterBusiness.BusinessMain.BusinessBoard;

public static class LetterNumber
{
    public static Letter GetLetterEnum(int letter)
    {
        return letter switch
        {
            0 => Letter.A,
            1 => Letter.B,
            2 => Letter.C,
            3 => Letter.D,
            4 => Letter.E,
            5 => Letter.F,
            6 => Letter.G,
            7 => Letter.H,
            _ => throw new ArgumentException(Messages.UnknownLetter)
        };
    }
    
    public static string GetNumber(int number)
    {
        return number switch
        {
            0 => "1",
            1 => "2",
            2 => "3",
            3 => "4",
            4 => "5",
            5 => "6",
            6 => "7",
            7 => "8",
            _ => throw new ArgumentException(Messages.UnknownNumber)
        };
    }
    
    public static Number2 GetNumberEnum(int number)
    {
        return number switch
        {
            0 => Number2.N1,
            1 => Number2.N2,
            2 => Number2.N3,
            3 => Number2.N4,
            4 => Number2.N5,
            5 => Number2.N6,
            6 => Number2.N7,
            7 => Number2.N8,
            _ => throw new ArgumentException(Messages.UnknownNumber)
        };
    }
    
    public static Number2 GetNumberEnumReverse(int number)
    {
        return number switch
        {
            0 => Number2.N8,
            1 => Number2.N7,
            2 => Number2.N6,
            3 => Number2.N5,
            4 => Number2.N4,
            5 => Number2.N3,
            6 => Number2.N2,
            7 => Number2.N1,
            _ => throw new ArgumentException(Messages.UnknownNumber)
        };
    }
    
    public static int GetNumberIndexReverse(Number2 number)
    {
        return number switch
        {
            Number2.N1 => 7,
            Number2.N2 => 6,
            Number2.N3 => 5,
            Number2.N4 => 4,
            Number2.N5 => 3,
            Number2.N6 => 2,
            Number2.N7 => 1,
            Number2.N8 => 0,
            _ => throw new ArgumentException(Messages.UnknownNumber)
        };
    }
    
    public static bool IsLetterNumberIllegal(int index) {
        return index is < 0 or > 7;
    }
}