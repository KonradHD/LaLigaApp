import kagglehub

def import_data():

    path1 = kagglehub.dataset_download("hubertsidorowicz/football-players-stats-2024-2025")
    path2 = kagglehub.dataset_download("mohamedsewid/soccer-forwards-performance-and-market-value-2025")

    print("Path to dataset files:", path1)
    print("Path to dataset files:", path2)
    return path1

import_data()
