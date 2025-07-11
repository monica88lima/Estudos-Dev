const apiKey = document.getElementById("apiKey");
const gameSelecionado = document.getElementById("gameSelecionado");
const perguntaFeita = document.getElementById("perguntaFeita");
const botaoPergunta = document.getElementById("botaoPergunta");
const aiResponse = document.getElementById("aiResponse");
const form = document.getElementById("form");

const markdownToHTML = (text) => {
  const converter = new showdown.Converter();
  return converter.makeHtml(text);
};

const perguntarAI = async (questao, game, apiKeyinput) => {
  const model = "gemini-2.0-flash";
  const geminiURL = `https://generativelanguage.googleapis.com/v1beta/models/${model}:generateContent?key=${apiKeyinput}`;
  const pergunta = `
    ## Especialidade
    Você é um especialista assistente de meta para o jogo ${game}

    ## Tarefa
    Você deve responder as perguntas do usuário com base no seu conhecimento do jogo, estratégias, build e dicas
    ## Regras
    - Se você não sabe a resposta, responda com 'Não sei' e não tente inventar uma resposta.
    - Se a pergunta não está relacionada ao jogo, responda com 'Essa pergunta não está relacionada ao jogo'
    - Considere a data atual ${new Date().toLocaleDateString()}
    - Faça pesquisas atualizadas sobre o patch atual, baseado na data atual, para dar uma resposta coerente.
    - Nunca responsda itens que vc não tenha certeza de que existe no patch atual.
    ## Resposta
    - Economize na resposta, seja direto e responda no máximo 500 caracteres
    - Responda em markdown
    - Não precisa fazer nenhuma saudação ou despedida, apenas responda o que o usuário está querendo.
    ## Exemplo de resposta
    pergunta do usuário: Melhor combinação para corridas online
    resposta: 
**Personagem:** Wario  
**Kart:** Flame Rider  
**Pneus:** Slick  
**Asa:** Cloud Glider  
**Motivo:** Ótimo equilíbrio entre velocidade e peso, ideal para pistas com curvas largas e competição online.

    ---
    Aqui está a pergunta do usuário: ${questao}
  `;
  console.log(pergunta);
  const contents = [
    {
      role: "user",
      parts: [
        {
          text: pergunta,
        },
      ],
    },
  ];

  const tools = [
    {
      google_search: {},
    },
  ];

  // chamada API
  const response = await fetch(geminiURL, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      contents,
      tools,
    }),
  });

  const data = await response.json();
  console.log("Resposta da API:", data);
  return data.candidates[0].content.parts[0].text;
};
const enviarFormulario = async (event) => {
  event.preventDefault();
  const apiKeyinput = apiKey.value;
  const game = gameSelecionado.value;
  const questao = perguntaFeita.value;

  if (!game || !questao) {
    alert("Por favor, preencha todos os campos.");
    return;
  }

  botaoPergunta.disabled = true;
  botaoPergunta.textContent = "Perguntando...";
  botaoPergunta.classList.add("loading");

  try {
    const text = await perguntarAI(questao, game, apiKeyinput);
    aiResponse.querySelector(".response-content").innerHTML =
      markdownToHTML(text);
    aiResponse.classList.remove("hidden");
  } catch (error) {
    console.log("Erro:", error);
  } finally {
    botaoPergunta.disabled = false;
    botaoPergunta.textContent = "Perguntar";
    botaoPergunta.classList.remove("loading");
  }
};

form.addEventListener("submit", enviarFormulario);
