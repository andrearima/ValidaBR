namespace Valida.Br;

public class RG { }
public class CNPJ { }

/// <summary>
/// Cadastro de Pessoa Física
/// </summary>
public class CPF
{
    private static int[] _sequencia = [10, 9, 8, 7, 6, 5, 4, 3, 2];
    /// int.MaxValue = 214_748_836_47
    private const long maxSize = 999_999_999_99;
    private readonly string _cpf;
    private readonly long _cpfLong;
    private int _primeiroDigitoVerificadorCalculado;
    private int _segundoDigitoVerificadorCalculado;


    /// 000.000.000-00
    /// xxx.xxx.xx0-00 --> X - Digitos definidos pela Receita Federal
    /// 000.000.00x-00 --> X - Região Fiscal Emissora do CPF
    /// 000.000.000-xx --> X - Primeiro e segundo digito verificador
    /// 

    public CPF(int intCpf)
    {
        _cpfLong = intCpf;
        _cpf = intCpf.ToString("D11");
    }

    public CPF(long longCpf)
    {
        _cpfLong = longCpf;
        _cpf = longCpf.ToString("D11");
    }

    private int[]? _digitos = null;
    public int[] Digitos
    {
        get
        {
            _digitos ??= _cpfLong.ToString("D11").Select(x => int.Parse(x.ToString())).ToArray();

            return _digitos;
        }
    }

    public int PrimeiroDigitoVerificador
    {
        get
        {
            return Digitos[9];
        }
    }

    public int SegundoDigitoVerificador
    {
        get
        {
            return Digitos[10];
        }
    }

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(_cpf))
            return false;

        if (_cpf.Length < 11)
            return false;

        if (_cpfLong >= maxSize)
            return false;

        CalcularDigitoVerificador();

        if (PrimeiroDigitoVerificador != _primeiroDigitoVerificadorCalculado)
            return false;

        if (SegundoDigitoVerificador != _segundoDigitoVerificadorCalculado)
            return false;

        return true;
    }

    private void CalcularDigitoVerificador()
    {
        var somaPrimeiroAlgarismo = 0;
        var somaSegundoAlgarismo = 0;

        for (int i = 0; i <= 9; i++)
        {
            somaPrimeiroAlgarismo += Digitos[i] * _sequencia[i];
            somaSegundoAlgarismo += Digitos[i + 1] * _sequencia[i];
        }

        var resto = somaPrimeiroAlgarismo % 11;
        _primeiroDigitoVerificadorCalculado = ObterDigitoVerificador(resto);

        resto = somaSegundoAlgarismo % 11;
        _segundoDigitoVerificadorCalculado = ObterDigitoVerificador(resto);
    }

    private int ObterDigitoVerificador(int resto)
    {
        if (resto == 0 || resto == 1)
            return 0;

        return 11 - resto;
    }
}

public enum RegiaoFiscal
{
    CentroOesteAmpliado = 1, // DF, GO, MS, MT, TO
    Norte = 2,               // AC, AM, AP, PA, RO, RR
    NordesteSetentrional = 3,// CE, MA, PI
    NordesteOriental = 4,    // AL, PB, PE, RN
    NordesteMeridional = 5,  // BA, SE
    MinasGerais = 6,         // MG
    SudesteLeste = 7,        // ES, RJ
    SudestePaulista = 8,     // SP
    SulCentroNorte = 9,      // PR, SC
    SulMeridional = 0,       // RS
}

public enum UF
{
    DF, GO, MS, MT, TO,
    AC, AM, AP, PA, RO, RR,
    CE, MA, PI,
    AL, PB, PE, RN,
    BA, SE,
    MG,
    ES, RJ,
    SP,
    PR, SC,
    RS
}