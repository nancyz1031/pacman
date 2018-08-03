import * as React from 'react';
import { PacMan } from './PacMan';
import { variables } from '../variables';
import * as contract from '../contract';

export interface StateProps {
    player: contract.Player;
}

export interface DispatchProps {
    move(up: boolean, right: boolean, down: boolean, left: boolean): void
}

function init(dispatchProps: DispatchProps) {
    const dic: { [key: string]: boolean; } = {};

    document.addEventListener('keydown', (event) => {
        switch (event.key) {
            case "ArrowRight":
            case "ArrowLeft":
            case "ArrowUp":
            case "ArrowDown":
            case 'w':
            case 'd':
            case 's':
            case 'a':
                dic[event.key] = true;
                break;
        }
    });

    document.addEventListener('keyup', (event) => {
        switch (event.key) {
            case "ArrowRight":
            case "ArrowLeft":
            case "ArrowUp":
            case "ArrowDown":
            case 'w':
            case 'd':
            case 's':
            case 'a':
                dic[event.key] = false;
                break;
        }
    });

    window.setInterval(() => {
        dispatchProps.move(dic['w'] || dic['ArrowUp'], dic['d'] || dic['ArrowRight'], dic['s'] || dic['ArrowDown'], dic['a'] || dic['ArrowLeft']);
    }, variables.refreshFrequency);
}

export class Player extends React.Component<StateProps & DispatchProps, {}> {
    componentDidMount() {
        init(this.props);
    }

    public render() {
        const props = this.props;
        return <PacMan player={props.player} current={true} />
    }
}
