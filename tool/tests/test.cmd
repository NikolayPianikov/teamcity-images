TeamCity.Docker\bin\Debug\netcoreapp3.1\TeamCity.Docker.exe generate -s configs -t generated
TeamCity.Docker\bin\Debug\netcoreapp3.1\TeamCity.Docker.exe build -i 23 -s configs -c context
TeamCity.Docker\bin\Debug\netcoreapp3.1\TeamCity.Docker.exe push -i 23 -u nikolayp -p 88e7198e-433d-4035-9e8a-956f9a3bdeae -c