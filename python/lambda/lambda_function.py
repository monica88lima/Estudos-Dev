import json
import logging
from datetime import datetime
logger = logging.getLogger()
logger.setLevel(logging.INFO)

def parse_datetime(value: str):
    if not value:
        return None
    return datetime.strptime(value, "%Y-%m-%dT%H:%M:%S")

def send_to_sns(topic_name: str, message: dict):
    logger.info(f"[MOCK SNS] Enviando para o tópico: {topic_name}")
    logger.info(f"[MOCK SNS] Mensagem: {json.dumps(message, default=str)}")

def lambda_handler(event, context):
    logger.info("Lambda acionada")
    logger.info(f"Evento recebido: {json.dumps(event)}")

    resultado_processado = []

    for record in event.get("Records", []):
        body_str = record.get("body")
        payload = json.loads(body_str)

        logger.info(f"Processando mensagem: {payload}")

        # Identificar tipo de objeto
        if "databloqueio" in payload:
            processar_bloqueio(payload)

        elif "datadesbloqueio" in payload:
            processar_desbloqueio(payload)

        else:
            logger.warning("Mensagem recebida com formato desconhecido")

        # Adiciona o payload processado ao resultado
        resultado_processado.append(payload)

    return {
        "statusCode": 200,
        "body": json.dumps(resultado_processado)  # Retorna os dados processados no body
    }

def processar_bloqueio(payload: dict):
    logger.info("Processando evento de BLOQUEIO")

    payload["databloqueio"] = normalize_date(payload.get("databloqueio"))
    payload["hora_bloqueio"] = normalize_hour(payload.get("hora_bloqueio"))

    # Atualiza o campo data_criacao para o formato datetime
    payload["data_criacao"] = normalize_datetime(payload.get("data_criacao"))

    send_to_sns(
        topic_name="enviar-dados-bloqueio",
        message=payload
    )

def processar_desbloqueio(payload: dict):
    logger.info("Processando evento de DESBLOQUEIO")

    payload["datadesbloqueio"] = normalize_date(payload.get("datadesbloqueio"))

    # Corrigindo a normalização da hora para o formato HH:MM
    payload["hora_desbloqueio"] = normalize_hour(payload.get("hora_desbloqueio"))

    payload["data_criacao"] = normalize_datetime(payload.get("data_criacao"))

    send_to_sns(
        topic_name="enviar-dados-desbloqueio",
        message=payload
    )

def normalize_date(value):
    """
    Recebe string ou datetime e retorna data no formato yyyy-mm-dd
    """
    if not value:
        logger.warning("normalize_date recebeu valor vazio")
        return None

    try:
        if isinstance(value, datetime):
            return value.strftime("%Y-%m-%d")

        if isinstance(value, str):
            formatos_aceitos = [
                "%Y-%m-%dT%H:%M:%S",
                "%Y-%m-%d %H:%M:%S",
                "%Y-%m-%d"
            ]

            for formato in formatos_aceitos:
                try:
                    data = datetime.strptime(value, formato)
                    return data.strftime("%Y-%m-%d")
                except ValueError:
                    continue

            logger.error(f"Formato de data inválido: {value}")
            return None

        logger.error(f"Tipo não suportado para data: {type(value)}")
        return None

    except Exception as e:
        logger.exception(f"Erro inesperado ao normalizar data: {value}")
        return None

def normalize_hour(value):
    """
    Recebe string ou datetime e retorna hora no formato HH:MM
    """
    if not value:
        logger.warning("normalize_hour recebeu valor vazio")
        return None

    try:
        logger.info(f"Valor recebido para normalizar hora: {value}")

        if isinstance(value, datetime):
            normalized = value.strftime("%H:%M")
            logger.info(f"Hora normalizada (datetime): {normalized}")
            return normalized

        if isinstance(value, str):
            formatos_aceitos = [
                "%Y-%m-%dT%H:%M:%S",
                "%Y-%m-%d %H:%M:%S",
                "%H:%M:%S",
                "%H:%M"
            ]

            for formato in formatos_aceitos:
                try:
                    logger.info(f"Tentando normalizar com o formato: {formato}")
                    hora = datetime.strptime(value, formato)
                    normalized = hora.strftime("%H:%M")
                    logger.info(f"Hora normalizada (str): {normalized}")
                    return normalized
                except ValueError:
                    logger.info(f"Falha ao normalizar com o formato: {formato}")
                    continue

            logger.error(f"Formato de hora inválido: {value}")
            return None

        logger.error(f"Tipo não suportado para hora: {type(value)}")
        return None

    except Exception:
        logger.exception(f"Erro inesperado ao normalizar hora: {value}")
        return None

def normalize_datetime(value):
    """
    Recebe string ou datetime e retorna data e hora no formato yyyy-mm-ddTHH:MM:SS
    """
    if not value:
        logger.warning("normalize_datetime recebeu valor vazio")
        return None

    try:
        if isinstance(value, datetime):
            return value.strftime("%Y-%m-%dT%H:%M:%S")

        if isinstance(value, str):
            formatos_aceitos = [
                "%Y-%m-%dT%H:%M:%S",
                "%Y-%m-%d %H:%M:%S",
                "%Y-%m-%d"
            ]

            for formato in formatos_aceitos:
                try:
                    data = datetime.strptime(value, formato)
                    return data.strftime("%Y-%m-%dT%H:%M:%S")
                except ValueError:
                    continue

            logger.error(f"Formato de datetime inválido: {value}")
            return None

        logger.error(f"Tipo não suportado para datetime: {type(value)}")
        return None

    except Exception as e:
        logger.exception(f"Erro inesperado ao normalizar datetime: {value}")
        return None
    
if __name__ == "__main__":
    with open("event_mock.json") as f:
        event = json.load(f)

    lambda_handler(event, None)