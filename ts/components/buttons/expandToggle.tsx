import * as React from 'react';
import Logger from '../../logger';
interface IExpandToggleState {
    supplimentalClass: string;
}

interface IExpandToggleProps {
    style?: React.CSSProperties;
    iconColor?: string;
    onClick: () => void;
}
export default class ExpandToggle extends React.Component<IExpandToggleProps, IExpandToggleState> {
    constructor(props) {
        super(props);
        this.state = {
            supplimentalClass: 'restored'
        };
    }

    render() {
        return (
            <div 
                className={`expand-toggle-container`}
                style={this.props.style}
                onClick={() => this.toggleClass()}
            >
                <svg
                    className={`expand-toggle ${this.state.supplimentalClass}`}
                    width="100"
                    height="100"
                    viewBox="0 0 100 100"
                    xmlns="http://www.w3.org/2000/svg"
                >         
                    <path
                        className="expand-arrow-icon"
                        d={`M 45 35
                            L 20 55
                            L 85 55`}
                        fill="transparent"
                        stroke={this.props.iconColor || "rgba(21,221,221, 0.75)"}
                        strokeWidth="5"
                        strokeLinecap="round"
                    />
                </svg>
            </div>
        );
    }

    private toggleClass() {
        Logger.Debug('ExpandToggle', 'toggleClass', this.state.supplimentalClass);
        this.setState((prev: IExpandToggleState, props) => {
            return {
                supplimentalClass: prev.supplimentalClass == 'restored' ? 'reversed' : 'restored'
            };
        }, () => {
            this.props.onClick();
        });
    }
}