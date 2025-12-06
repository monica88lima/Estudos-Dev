# DescompactaECopiaFotos

Este projeto contém dois scripts Python para facilitar o trabalho com arquivos de fotos:

- `descompacta.py`: Descompacta arquivos ZIP contendo fotos.
- `localiza-fotos.py`: Localiza fotos em diretórios e copia para uma pasta de saída.

## Estrutura do Projeto

```
descompacta.py
localiza-fotos.py
saida/
```

## Requisitos
- Python 3.x

## Como usar

### 1. Descompactar arquivos ZIP
Execute o script `descompacta.py` para descompactar arquivos ZIP que contenham fotos.

```powershell
python descompacta.py
```

### 2. Localizar e copiar fotos
Execute o script `localiza-fotos.py` para localizar fotos em diretórios e copiá-las para a pasta `saida`.

```powershell
python localiza-fotos.py
```

A pasta `saida` será criada automaticamente se não existir.

## Observações
- Certifique-se de que os arquivos ZIP estejam no diretório correto antes de executar o script de descompactação.
- Os scripts podem ser adaptados conforme a necessidade para diferentes formatos de imagem ou critérios de busca.

## Licença
Este projeto é de uso pessoal e educacional.

**Atenção:** O script `localiza-fotos.py` possui caminhos de pastas definidos diretamente no código (hardcoded):
	- `pasta_inicial = Path('D:/fotos diversas')` (pasta onde estão os arquivos para buscar)
	- `pasta_destino = Path('C:/dev/python/saida')` (pasta onde serão copiadas as fotos)
Esses caminhos devem ser alterados conforme o ambiente e as pastas do usuário antes de executar o script.
