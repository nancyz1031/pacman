import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../store';
import { Login, StateProps, DispatchProps } from './Login';
import { serverFuncs } from '../serverFuncs';

function mapStateToProps(state: ApplicationState): StateProps {
    return {
    };
}

function mapDispatchToProps(dispatch: Dispatch<any>): DispatchProps {
    return {
        login: (userName: string) => {
            serverFuncs.playerJoin(userName);
        }
    };
}

export const LoginContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(Login);
