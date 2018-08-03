import * as signalR from '@aspnet/signalr';
import { Position } from './contract';

function getConnection(): signalR.HubConnection {
    // TODO: Shouldn't store connection to window object
    return (window as any).connection as signalR.HubConnection;
}

export const serverFuncs = {
    playerMoveTo: (position: Position) => {
        getConnection().invoke("PlayerMoveTo", position)
            .catch(err => console.error(err.toString()));
    },
    playerJoin: (userName: string) => {
        getConnection().invoke("PlayerJoin", userName)
            .catch(err => console.error(err.toString()));
    }
}