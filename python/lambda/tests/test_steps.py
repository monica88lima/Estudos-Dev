import json
import re
import pytest
from pytest_bdd import scenarios, given, when, then, parsers

from lambda_function import lambda_handler

scenarios("features/bloqueio.feature")

scenarios("features/desbloqueio.feature")

@pytest.fixture
def context():
    return {}


@given(parsers.parse("que recebo um evento do SQS do tipo \"{sns}\""))
def receber_evento_sqs(sns, context):
    # Simula o recebimento de um evento do SQS baseado no tipo
    if sns == "bloqueio":
        context["evento"] = {
            "Records": [
                {
                    "body": json.dumps({
                        "identificado": "123",
                        "codigo": "BLOQ01",
                        "databloqueio": "2023-10-01T15:40:00",
                        "data_criacao": "2026-01-30T13:00:00",
                        "hora_bloqueio": "15:40:00"
                    })
                }
            ]
        }
    elif sns == "desbloqueio":
        context["evento"] = {
            "Records": [
                {
                    "body": json.dumps({
                        "identificado": "456",
                        "codigo": "DESB01",
                        "datadesbloqueio": "2023-11-15T15:40:00",
                        "data_criacao": "2026-01-30T13:10:00",
                        "hora_desbloqueio": "15:40:00",
                        "idOrdem": "ORD998",
                        "divergencia": False
                    })
                }
            ]
        }


@when("a lambda processa o evento")
def processar_evento_lambda(context):
    print("Evento recebido:", context["evento"])    
    context["resultado"] = lambda_handler(context["evento"], None)
    print("Resultado da lambda:", context["resultado"])  


@then(parsers.parse("os dados devem ser enviados para o tópico \"{topico}\""))
def verificar_envio_topico(context, topico):
    # Verifica se os dados foram enviados para o tópico correto
    resultado = context["resultado"]
    print("Resultado da lambda:", resultado)

    assert resultado["statusCode"] == 200, "A lambda não processou o evento corretamente."

    # Decodifica o JSON do body retornado pela Lambda
    body = json.loads(resultado["body"])
    print("Dados processados retornados pela Lambda:", body)

    # Verifica se os dados processados estão corretos
    for evento in body:
        if "databloqueio" in evento:
            assert evento["codigo"].startswith("BLOQ"), "Código de bloqueio incorreto."
            assert evento["hora_bloqueio"].count(":"), "Formato de hora incorreto para bloqueio."
        elif "datadesbloqueio" in evento:
            assert evento["codigo"].startswith("DESB"), "Código de desbloqueio incorreto."
            assert evento["hora_desbloqueio"].count(":"), "Formato de hora incorreto para desbloqueio."

    print("Os dados foram enviados corretamente para o tópico.")


@then(parsers.parse("a formatacao da data deve estar no formato aaaa-mm-dd \"{data_inicial}\""))
def verificar_formatacao_data(context, data_inicial):
    # Verifica se a data está no formato correto no evento processado
    evento = context["evento"]
    print("Evento recebido:", evento)

    # Extrai o corpo do evento
    body = json.loads(evento["Records"][0]["body"])

    # Verifica se a data está no formato correto
    if "databloqueio" in body:
        assert re.match(r"\d{4}-\d{2}-\d{2}", body["databloqueio"]), f"Formato incorreto: {body['databloqueio']}"
        assert body["databloqueio"].startswith(data_inicial), f"Esperado: {data_inicial}, Obtido: {body['databloqueio']}"
    elif "datadesbloqueio" in body:
        assert re.match(r"\d{4}-\d{2}-\d{2}", body["datadesbloqueio"]), f"Formato incorreto: {body['datadesbloqueio']}"
        assert body["datadesbloqueio"].startswith(data_inicial), f"Esperado: {data_inicial}, Obtido: {body['datadesbloqueio']}"


@then(parsers.parse("a formatacao da hora deve estar no formato hh:mm \"{hora_inicial}\""))
def verificar_formatacao_hora(context, hora_inicial):
    # Verifica se a hora está no formato correto no evento processado pela Lambda
    resultado = context["resultado"]
    print("Resultado da lambda:", resultado)

    # Extrai o corpo do resultado da Lambda
    body = json.loads(resultado["body"])
    print("Corpo do resultado da lambda:", body)

    if "hora_bloqueio" in body:
        print("Hora bloqueio processada:", body["hora_bloqueio"])
        assert re.match(r"\d{2}:\d{2}", body["hora_bloqueio"]), f"Formato incorreto: {body['hora_bloqueio']}"
        assert body["hora_bloqueio"] == hora_inicial, f"Esperado: {hora_inicial}, Obtido: {body['hora_bloqueio']}"
    elif "hora_desbloqueio" in body:
        print("Hora desbloqueio processada:", body["hora_desbloqueio"])
        assert re.match(r"\d{2}:\d{2}", body["hora_desbloqueio"]), f"Formato incorreto: {body['hora_desbloqueio']}"
        assert body["hora_desbloqueio"] == hora_inicial, f"Esperado: {hora_inicial}, Obtido: {body['hora_desbloqueio']}"


@then(parsers.parse("a formatacao da data de criacao deve estar no formato aaaa-mm-dd \"{data_criacao}\""))
def verificar_formatacao_data_criacao(context, data_criacao):
    # Verifica se a data de criação está no formato correto no evento processado
    evento = context["evento"]
    print("Evento recebido:", evento)

    # Extrai o corpo do evento
    body = json.loads(evento["Records"][0]["body"])

    assert re.match(r"\d{4}-\d{2}-\d{2}", body["data_criacao"]), f"Formato incorreto: {body['data_criacao']}"
    assert body["data_criacao"].startswith(data_criacao), f"Esperado: {data_criacao}, Obtido: {body['data_criacao']}"