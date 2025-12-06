import os
import zipfile

# Solicita os caminhos ao usuário
caminho_downloads = input('Digite o caminho onde estão os arquivos ZIP: ').strip()
caminho_destino_base = input('Digite o caminho onde os arquivos devem ser descompactados: ').strip()


os.makedirs(caminho_destino_base, exist_ok=True)


arquivos_takeout = [
    nome for nome in os.listdir(caminho_downloads)
    if nome.lower().startswith('takeout') and nome.lower().endswith('.zip')
]
#mostra a contagem de arquivos encontrados
print(f'📦 Encontrados {len(arquivos_takeout)} arquivos ZIP com prefixo "takeout".')

# Descompacta cada arquivo em uma subpasta separada
for nome_arquivo in arquivos_takeout:
    caminho_zip = os.path.join(caminho_downloads, nome_arquivo)
    nome_base = os.path.splitext(nome_arquivo)[0]  # remove .zip
    caminho_destino_individual = os.path.join(caminho_destino_base, nome_base)

    os.makedirs(caminho_destino_individual, exist_ok=True)
    print(f'Descompactando {nome_arquivo} em {caminho_destino_individual}')

    with zipfile.ZipFile(caminho_zip, 'r') as zip_ref:
        zip_ref.extractall(caminho_destino_individual)

print('Todos os arquivos foram descompactados com sucesso!')
