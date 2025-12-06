import os
from pathlib import Path
import shutil

# Configurações
pasta_inicial = Path('D:/fotos diversas') # pasta onde estão os arquivos para buscar
pasta_destino = Path('C:/dev/python/saida')# pasta onde serão copiadas as fotos


pasta_destino.mkdir(parents=True, exist_ok=True)

# Lista todos os itens na pasta inicial
itens = [item for item in pasta_inicial.iterdir() if item.is_dir()]

for item in itens:
    # Procura por subpasta que começa com 'takeout'
    subpastas_takeout = [p for p in item.iterdir() if p.is_dir() and p.name.lower().startswith('takeout')]
    if not subpastas_takeout:
        continue
    for takeout_pasta in subpastas_takeout:
        # Procura por pasta 'Google Fotos' 
        google_fotos_pasta = None
        for root, dirs, files in os.walk(takeout_pasta):
            for d in dirs:
                if d.lower() == 'google fotos':
                    google_fotos_pasta = Path(root) / d
                    break
            if google_fotos_pasta:
                break
        if not google_fotos_pasta:
            continue
        # Procura por arquivos .jpg dentro de Google Fotos 
        jpgs = list(google_fotos_pasta.rglob('*.mp4'))
        if not jpgs:
            continue
        # Copia os arquivos para pasta 
        for jpg in jpgs:
            destino = pasta_destino / jpg.name
            shutil.copy2(jpg, destino)
            try:
                jpg.unlink()
            except Exception as e:
                print(f'Erro ao apagar {jpg}: {e}')
        print(f'Copiados {len(jpgs)} arquivos de {google_fotos_pasta}')
print('Processo finalizado.')
