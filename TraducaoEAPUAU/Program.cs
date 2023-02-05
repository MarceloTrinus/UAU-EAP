// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using TraducaoEAPUAU;


//Abrir aquivo com todos os centros de custos]

StreamReader rd = new StreamReader(@"C:\Users\marcelo.melo\source\repos\TraducaoEAPUAU\TraducaoEAPUAU\EAP.csv", Encoding.Default, true);

//Lista totais
List<UauInsumo> Insumos = new();
List<UauComposicao> composicoes = new();
List<CentroDeCusto> centros = new();

string linha = null;
string[] line = null;

//Criar lista de tradução
while ((linha = rd.ReadLine()) != null)
{
    line = linha.Split(';');
    if (line[4] == "-1")
    {
        centros.Add(new CentroDeCusto
        {
            Cod = line[2],
            Description = line[5],
        });

        continue;
    }
    else if (line[4] == "")
    {
        composicoes.Add(new UauComposicao
        {
            CodCentro = line[2],
            CodComp = line[3],
            Description = line[5],
        });

        continue;
    }

    Insumos.Add(new UauInsumo
    {
        CodCentro = line[2],
        CodComp = line[3],
        CodInsumo = line[4],
        Description = line[5],
    });

}



//Adiciona Insumos as composições 
if (composicoes.Count > 0)
{
    foreach (var item in composicoes)
    {
        //Lista de Inserção 
        List<UauInsumo> insertInsumo = new();
        Insumos.ForEach(insumo =>
        {
            if(insumo.CodComp == item.CodComp)
            {
                insertInsumo.Add(insumo);   
            }
        });

        item.Insumos = insertInsumo;
    }
}

//Adiciona Composições aos centros de custos 
if (centros.Count > 0)
{
    foreach (var item in centros)
    {
        //Lista de Inserção 
        List<UauComposicao> insertComp = new();
        composicoes.ForEach(comp =>
        {
            if (comp.CodCentro.Contains(item.Cod))
            {
                insertComp.Add(comp);
            }
        });

        item.Composicoes = insertComp;
    }
}

var i = 0;

//Serializa os dados
string jsonString =
    JsonSerializer.Serialize(centros, new JsonSerializerOptions { });

//Adiciona composições aos centros de custos 


//Adicionar item mapeado 

//Passar pelo atributos do SONAR 

//Criar Json de tradução 

//Salvar arquivo de Traduções


UauInsumo translator = new UauInsumo();

Console.WriteLine("Hello, SONAR!!!!!!");