FROM mcr.microsoft.com/mssql/server

COPY auth/auth_tables.sql auth_tables.sql

COPY entrypoint.sh entrypoint.sh
#RUN chmod +x entrypoint.sh

ENTRYPOINT [ "/bin/bash", "entrypoint.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]
