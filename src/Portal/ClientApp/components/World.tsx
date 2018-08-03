import * as React from 'react';
import { PlayerContainer } from './PlayerContainer';
import { variables } from '../variables';
import * as contract from '../contract';
import { DotsContainer } from './DotsContainer';
import { RanksContainer } from './RanksContainer';
import { OtherPlayersContainer } from './OtherPlayersContainer';
import { LoginContainer } from './LoginContainer';
import { MessagesContainer } from './MessagesContainer';

export interface StateProps {
    connected: boolean;
    world: contract.World;
}

export interface DispatchProps {

}

export class World extends React.Component<StateProps & DispatchProps, {}> {
    public render() {
        const props = this.props;
        if (!props.connected) {
            return <div>Connecting to server...</div>
        }

        const world = this.props.world;
        if (!world || !world.variables) {
            return <LoginContainer />;
        }

        const width = world.variables.worldWidth * variables.size;
        const height = world.variables.worldHeight * variables.size;
        return <div>
            <div className="tips">Use 'w', 'a', 's', 'd' to control your pac man movement and eat dot as much as you can! Refresh your webbrowser to restart your game session.</div>
            <div className="world-container">
                <div id="world" style={{ width: width, height: height }}>
                    <DotsContainer />
                    <OtherPlayersContainer />
                    <PlayerContainer />
                </div>
                <RanksContainer />
            </div>
            <MessagesContainer />
        </div>;
    }
}
