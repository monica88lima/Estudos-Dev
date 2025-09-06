using System.Text;

namespace QuizGenie.Service;

public static class QuizGenie_Service
{
    private static string GerarPrompt(string tema, string serieEscolar, int idade, string intencionalidade,
        string tipoPergunta, int qtdAlternativas)
    {

        var estruturaJson = GeraTextoRetornoQuestao(tipoPergunta);
        
        return $"""
                           Você é um prossional da educação, especialista em neurociencia e em formas de aprendizagem. Gere questões de âmbito educacional com base nos seguintes parâmetros:
                           Tema:{tema}
                           Série escolar: {serieEscolar}
                           Idade do aluno: {idade}
                           Intencionalidade pedagógica: {intencionalidade}
                           Tipo de questão: {tipoPergunta}
                           Quantidade de perguntas: {qtdAlternativas}
                           A questão deve ser clara, adequada ao nível de complexidade esperado para a idade e série informadas.
                           A resposta correta deve estar entre as alternativas e deve vir acompanhada de uma explicação pedagógica que justifique por que ela é a correta.
                           Retorne o resultado **estruturado em JSON**, com os seguintes objetos:
                           1. pergunta: texto da pergunta
                           2. alternativas: lista de alternativas com indicação da correta
                           3. justificativa: explicação da resposta correta
                           Exemplo de estrutura esperada no formato json:
                           {
                               estruturaJson
                           }
                           """;
    }

    public static string GerarPergunta(string tema, string serieEscolar, int idade, string intencionalidade,
        int tipoPergunta, int qtdAlternativas)
    {
       string tpPergunta = ObterDescricaoTipoPergunta(tipoPergunta);
        var pergunta = GerarPrompt(tema, serieEscolar, idade, intencionalidade, tpPergunta, qtdAlternativas);
        return pergunta;
    }
    private static string ObterDescricaoTipoPergunta(int tipo)
    {
        return (ETipoPergunta)tipo switch
        {
            ETipoPergunta.MultiplaEscolha   => "Múltipla Escolha",
            ETipoPergunta.VerdadeiroOuFalso => "Verdadeiro ou Falso",
            ETipoPergunta.Classificacao     => "Classificação",
            ETipoPergunta.CompletarFrase    => "Completar Frase",
            _ => "Tipo desconhecido"
        };
    }

    private static string GeraTextoRetornoQuestao(string tipoPergunta)
    {
        return  tipoPergunta switch
       {
           "Múltipla Escolha" => """
                                       {
                                           "pergunta": "Qual é o maior planeta do sistema solar?",
                                           "alternativas": [
                                               { "texto": "Terra", "correta": false },
                                               { "texto": "Júpiter", "correta": true },
                                               { "texto": "Marte", "correta": false },
                                               { "texto": "Vênus", "correta": false }
                                           ],
                                           "justificativa": "Júpiter é o maior planeta do sistema solar em termos de massa e volume"
                                       }
                                       """,

           "Verdadeiro ou Falso" =>  """
                                    {
                                        "pergunta": "A água cobre mais de 70% da superfície da Terra.",
                                        "alternativas": [
                                            { "texto": "Verdadeiro", "correta": true },
                                            { "texto": "Falso", "correta": false }
                                        ],
                                        "justificativa": "A maior parte da superfície terrestre é coberta por oceanos, mares e outros corpos d'água, totalizando cerca de 71%."
                                    }
                                    """, 
           "Classificação"  => """
                             {
                                 "pergunta": "Classifique os seguintes materiais como recicláveis ou não recicláveis.",
                                 "alternativas": [
                                     { "texto": "Garrafa plástica", "correta": true },
                                     { "texto": "Papelão", "correta": true },
                                     { "texto": "Casca de banana", "correta": false },
                                     { "texto": "Lata de alumínio", "correta": true },
                                     { "texto": "Guardanapo usado", "correta": false }
                                 ],
                                 "justificativa": "Materiais recicláveis são aqueles que podem ser reaproveitados industrialmente. Orgânicos e contaminados geralmente não entram nesse grupo."
                             }
                             """,

           "Completar Frase" => """
                               {
                                   "pergunta": "Complete a frase: Para economizar água ao escovar os dentes, devemos ______.",
                                   "alternativas": [
                                       { "texto": "deixar a torneira aberta o tempo todo", "correta": false },
                                       { "texto": "fechar a torneira enquanto escovamos", "correta": true },
                                       { "texto": "usar mais pasta de dente", "correta": false },
                                       { "texto": "escovar os dentes no chuveiro", "correta": false }
                                   ],
                                   "justificativa": "Fechar a torneira durante a escovação evita desperdício e é uma prática sustentável."
                               }
                               """,

           _ => throw new ArgumentOutOfRangeException(nameof(tipoPergunta), "Tipo de pergunta não reconhecido.")
       };

   }

    
}