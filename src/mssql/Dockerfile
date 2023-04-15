FROM mcr.microsoft.com/mssql/server

COPY auth/* /
COPY cryptouserapis/* /

COPY entrypoint.sh entrypoint.sh
#RUN chmod +x entrypoint.sh

ENTRYPOINT [ "/bin/bash", "entrypoint.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]
