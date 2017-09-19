import * as React from 'react';
interface ExpandToggleState {
    supplimentalClass: string;
}

interface ExpandToggleProps {
    style?: React.CSSProperties;
    iconColor?: string;
    onClick: () => void;
}
export default class ExpandToggle extends React.Component<ExpandToggleProps, ExpandToggleState> {
    constructor(props) {
        super(props);
        this.state = {
            supplimentalClass: ''
        };
    }

    render() {
        return (
            <div 
                className={`expand-toggle-container`}
                style={this.props.style}
                onClick={this.props.onClick}
            >
                <svg
                    className="expand-toggle"
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
        this.setState((prev, props) => {
            if (prev.supplimentalClass == ' reversed') {
                return {supplimentalClass: ' restored'}
            }
            return {supplimentalClass: ' restored'}
        });
    }
}