# Hawks's Arges Juggernaut Integration

An optional SPT add-on connecting Hawks's M135 Arges with Juggernaut. The Juggernaut boss carries an Arges loaded with 7.62×51 mm Ultra Nosler and keeps his grenade launcher. While he fires the Arges, the belt replenishes without a reload. On death, the belt has a random number of rounds left for looting. Players wearing a powered Juggernaut exoskeleton get an additional recoil reduction with the Arges.

## Installation

Install SPT 4.1.6, Hawks's M135 Arges 1.1.0 or later, and Juggernaut 1.1.1 (compatible 1.1.x releases starting at 1.1.1). Extract the release ZIP into the SPT root folder containing EscapeFromTarkov.exe. Restart the server and client.

## Build

With the .NET 10 SDK installed, run both commands from the repository root, supplying the path to your SPT installation.

    dotnet build Client/Hawks.Arges.Juggernaut.Client.csproj -c Release "-p:SPTRoot=E:/Games/SPT"
    dotnet build Server/Hawks.Arges.Juggernaut.Server.csproj -c Release "-p:SPTRoot=E:/Games/SPT"

Server mod GUID is `com.hawks.arges.juggernaut`. The client plugin GUID is `com.hawks.arges.juggernaut.client`.
