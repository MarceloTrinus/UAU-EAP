using System.Text;
using System.Text.Json;
using System.Threading;
using EAPSONAR;

//Abrir arquivos com EAP / por centro de custo
IDictionary<string, string> Readers = new Dictionary<string, string>()
{

    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-ADM.csv", "DESPESAS ADMINISTRATIVAS"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-ALP.csv", "CUSTOS COM APROVACOES, LICENCIAMENTO, PROJETOS" },
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-APT.csv", "APORTE" },
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-CAQ.csv", "CUSTO COM AQUISICOES" },
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-CDR.csv", "CARTEIRA DE RECEBIVEIS"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-COM.csv", "DESPESAS COMERCIAIS"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-DIS.csv", "DISTRIBUICAO DE LUCRO"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-DPO.csv", "DESPESA POS OBRA"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-DPV.csv", "DESPESA COM POS VENDA"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-EMP.csv", "EMPRESTIMOS"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-MKT.csv", "DESPESAS COM MARKETING"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-ORBURB.csv", "OBRA-URBANISMO"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-ORBINC.csv", "OBRA-INCORPORACAO"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-FOLHA.csv", "FOLHA"},
    { @"C:\Users\marcelo.melo\Documents\GitRepository\UAU-EAP\TraducaoEAPUAU\BaseEAP\EAP-HOLDING.csv", "HOLDING"},
};

Task[] tasks = new Task[Readers.Count];

//Sincronizadores;
var semaphoreCentro = new SemaphoreSlim(0, 1);
var semaphoreServiços = new SemaphoreSlim(0, 1);
var semaphoreInsumos = new SemaphoreSlim(0, 1);

//Lista de Insumos, serviços e Centros de Custos
List<UauInsumo> Insumos = new();
List<UauComposicao> composicoes = new();
List<CentroDeCusto> centros = new();

int count = 0;
//Criar lista de tradução
foreach(KeyValuePair<string, string> key in Readers)
{
    tasks[count] = Task.Run(() =>
    {
        string linha;
        string[] line;

        StreamReader rd = new StreamReader(key.Key, Encoding.Default, true);

        while ((linha = rd.ReadLine()) != null)
        {
            line = linha.Split(';');
            if (line[3] == "-1")
            {
                semaphoreCentro.Wait();

                Console.WriteLine($"{key.Value} ---> Colocando Centro de Custo");

                centros.Add(new CentroDeCusto
                {
                    Id = Guid.NewGuid(),
                    Cod = line[2],
                    Description = line[5],
                });

                semaphoreCentro.Release();

                continue;
            }
            else if (line[4] == "")
            {
                semaphoreServiços.Wait();

                composicoes.Add(new UauComposicao
                {
                    Id = Guid.NewGuid(),
                    CodCentro = line[2],
                    CodComp = line[3],
                    Description = line[5],
                });

                semaphoreServiços.Release();

                continue;
            }

            semaphoreInsumos.Wait();

            Insumos.Add(new UauInsumo
            {
                Id = Guid.NewGuid(),
                CodCentro = line[2],
                CodComp = line[3],
                CodInsumo = line[4],
                Description = line[5],
            });

            semaphoreInsumos.Release();
        }

    });

    count++;
}

Thread.Sleep(500);

semaphoreCentro.Release();
semaphoreServiços.Release();
semaphoreInsumos.Release();

Task.WaitAll(tasks);

/*

//Adiciona Insumos as composições 
if (composicoes.Count > 0)
{
    foreach (var item in composicoes)
    {
        //Lista de Inserção 
        List<UauInsumo> insertInsumo = new();
        Insumos.ForEach(insumo =>
        {
            if (insumo.CodComp == item.CodComp)
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
*/

Console.WriteLine("Produzindo EAP SONAR !!!!!!");
