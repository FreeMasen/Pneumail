import * as React from 'react';

export default class ExpandToggle extends React.Component<any,any> {
    render() {
        return (
            <div 
                className="expand-toggle-container"
                style={this.props.style}
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
}