FROM python:3.11-slim

WORKDIR /app

RUN apt-get update && apt-get install -y libpq-dev gcc postgresql-client && rm -rf /var/lib/apt/lists/*

RUN pip install psycopg2-binary pandas openai tqdm

CMD ["bash", "/app/scripts/entrypoint.sh"]
