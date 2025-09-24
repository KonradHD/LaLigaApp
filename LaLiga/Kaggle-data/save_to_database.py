import sqlite3
import pandas as pd
import numpy as np


def get_database_connection(database_name):
    conn = sqlite3.connect(database_name)
    return conn

def retrieve_data(cursor, table_name, position):
    select_string = ""
    df = pd.DataFrame()
    if position == "GK": # GoalKeeper
        select_string = f"SELECT Rk, Player, Age, MP, Min, GA90, Saves, \"Save%\", CS, PKsv, WartoscRynkowa FROM {table_name} WHERE Pos like 'GK%';"
        cursor.execute(select_string)
        df = pd.DataFrame(cursor.fetchall(), columns=["Rk", "Player", "Age", "MP", "Min", "GA90", "Saves", "Save%", "CS", "PKsv", "WartoscRynkowa"])
    if position == "DF": # Defender 
        select_string = f"SELECT Rk, Player, Age, MP, Min, Gls, Int, Clr, Err, Recov, WartoscRynkowa FROM {table_name} WHERE Pos like 'DF%';"
        cursor.execute(select_string)
        df = pd.DataFrame(cursor.fetchall(), columns=["Rk", "Player", "Age", "MP", "Min", "Gls", "Int", "Clr", "Err", "Recov", "WartoscRynkowa"])
    if position == "FW": # napastnik
        select_string = f"SELECT Rk, Player, Age, MP, Min, Gls, Ast, xG, xAG, Sh, \"Tkl%\", \"Won%\", WartoscRynkowa FROM {table_name} WHERE Pos like 'FW%';"
        cursor.execute(select_string)
        df = pd.DataFrame(cursor.fetchall(), columns=["Rk", "Player", "Age", "MP", "Min", "Gls", "Ast", "xG", "xAG", "Sh", "Tkl%", "Won%", "WartoscRynkowa"])
    if position == "MF": # pomocnik
        select_string = f"SELECT Rk, Player, Age, MP, Min, Gls, Ast, PrgC, SoT, \"Cmp%\", KP, \"Tkl%\", Lost, Touches, WartoscRynkowa FROM {table_name} WHERE Pos like 'MF%';"
        cursor.execute(select_string)
        df = pd.DataFrame(cursor.fetchall(), columns=["Rk", "Player", "Age", "MP", "Min", "Gls", "Ast", "PrgC", "SoT", "Cmp%", "KP", "Tkl%", "Lost", "Touches", "WartoscRynkowa"])

    return df


def convert_value(value):
    value = value.replace("€", "")
    if "m" in value:
        return float(value.replace("m", "")) * 1000000
    if "k" in value:
        return float(value.replace("k", ""))* 1000
    if value != "-":
        return float(value)
    return np.nan


def save_to_database(db_name, table_name):
    file_path_stats = r"C:\Users\Konrad\OneDrive\Pulpit\Studia\Semestr4\PZ2\machine_learning_model\Kaggle-data\kagglehub\datasets\hubertsidorowicz\football-players-stats-2024-2025\versions\43\players_data_light-2024_2025.csv"
    file_path_value = r"C:\Users\Konrad\OneDrive\Pulpit\Studia\Semestr4\PZ2\machine_learning_model\Kaggle-data\kagglehub\datasets\mohamedsewid\soccer-forwards-performance-and-market-value-2025\versions\1\transfermarkt_players.csv"

    conn = get_database_connection(db_name)
    df_stats = pd.read_csv(file_path_stats, usecols=["Rk", "Player", "Pos", "Squad", "Comp", "Age", "MP", "Starts", "Min", 
                                         "Gls", "Ast", "CrdY", "CrdR", "xG", "xAG", "PrgC", "PrgP", "Sh", "SoT", 
                                         "Cmp%", "KP", "PPA", "Tkl", "Tkl%", "Lost", "Int", "Clr", "Err", "Touches", 
                                         "Carries", "Recov", "Won%", "GA90", "Saves", "Save%", "CS", "PKsv"])
    #df = df[df["Comp"] == "es La Liga"]
    df_value = pd.read_csv(file_path_value, usecols=["name", "value"])
    df_value["value"] = np.where(
        df_value["value"] == "-",
        np.nan,
        df_value["value"].apply(convert_value)
    )

    df_value.rename(columns={"value" : "WartoscRynkowa", "name" : "Player"}, inplace=True)

    df_stats["Tkl%"] = df_stats["Tkl%"].fillna(0)
    df_stats["Won%"] = df_stats["Won%"].fillna(0)
    df_stats["Save%"] = np.where(
        (df_stats["Pos"] == "GK") & (df_stats["Save%"].isna()),
        0,
        df_stats["Save%"]
    )

    df_output = df_stats.merge(df_value, how="left", on = "Player")
    df_output.to_sql(table_name, conn, if_exists='replace', index=False)
    
    conn.commit()
    cursor = conn.cursor()
    
    print(f"Data saved to {db_name} in table {table_name}")

    #retrieve_data(cursor, table_name, "FW")
    conn.close()

save_to_database(r"C:\Users\Konrad\OneDrive\Pulpit\Studia\Semestr4\PZ2\projekt\LaLigaApp\LaLiga\Data\laliga.db", "PlayerOverallStats")

