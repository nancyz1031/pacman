import { Action, Reducer } from 'redux';
import { ActionType } from './actions';

export const connectionStateActionActionCreators = {
    updateConnected: () => {
        return { type: ActionType.Connected, };
    },
};

export const reducer: Reducer<boolean> = (state: boolean = false, action: Action) => {
    switch (action.type) {
        case ActionType.Connected:
            return true;

        default:
            break;
    }

    return state;
};