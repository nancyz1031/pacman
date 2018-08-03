import { Position } from './contract';
import { variables } from './variables';

export const utility = {
    getColor: (index: number) => {
        const colors = variables.colors
        return colors[index % colors.length];
    }
}