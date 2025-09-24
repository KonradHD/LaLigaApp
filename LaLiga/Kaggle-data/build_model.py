import pandas as pd
from tensorflow import keras
from save_to_database import get_database_connection
from save_to_database import retrieve_data

def create_model(x_train, y_train):
    callback_es = keras.callbacks.EarlyStopping(patience = 5, min_delta = 0.01, verbose=1)
    normalizer = keras.layers.Normalization()
    normalizer.adapt(x_train)
    
    
    model = keras.Sequential([
        normalizer,
        keras.Dense(255),
        keras.Dense(255),
        keras.Dense(255),
        keras.Dense(1)
    ])

    model.compile(loss = "mse", optimizer = "adam", metrics = ["mae"])

    model.fit(
        x=x_train, 
        y=y_train,
        epochs=20,
        callbacks=[callback_es]
    )


def get_results(db_name):
    positions = ["GK", "FW", "DF", "MF"]
    conn = get_database_connection(db_name)
    coursor = conn.cursor()

    for pos in positions: 
        df = retrieve_data(coursor, "PlayerOverallStats", pos)
        print(df.columns.tolist())
        df_train = df[~df["WartoscRynkowa"].isna()]
        x_train = df_train.drop(columns=["WartoscRynkowa"])
        y_train = df_train["WartoscRynkowa"]
        df_test = df[df["WartoscRynkowa"].isna()]
        df_test.drop(columns=["WartoscRynkowa"], inplace=True)
        print(df_train.shape, df_test.shape)

get_results(r"C:\Users\Konrad\OneDrive\Pulpit\Studia\Semestr4\PZ2\projekt\LaLigaApp\LaLiga\Data\laliga.db")