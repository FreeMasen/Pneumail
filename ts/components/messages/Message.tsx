import * as React from 'react';
import ExpandedToggle from '../buttons/expandToggle'
interface IMessageState {
    toggled?: boolean;
}

interface IMessageProps extends React.Props<HTMLDivElement> {
    subject?: string;
    sender?: string;
    recipients?: string[];
    coppied?: string[];
    blindCoppied?: string[];
    content: string;
}

export default class Message extends React.Component<IMessageProps, IMessageState> {
    constructor(props) {
        super(props);
        this.state = {
            toggled: false,
        }
    }
    render() {
        let iconClass = this.state.toggled ? 'icon-container toggled' : 'icon-container'
        return (
            <div className="message paper"
                onClick={ev => this.onClick(ev)}
            >
                <div className="subject-line">
                    <div className={iconClass}>
                        <svg
                            viewBox="0 0 100 100"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                className="open-toggle"
                                d={`M 5 5
                                    l 90 45
                                    l -90 45
                                `}
                            />
                        </svg>
                    </div>
                    <span>{this.props.sender}</span>
                    <span>{this.props.subject}</span>
                </div>
                {this.state.toggled ?
                    <div className="message-container">
                        <span className="message-text">{this.props.content}</span>
                    </div>
                : null}
            </div>
        );
    }

    onClick = (ev) => {
        this.setState((prev, props) => {
            return {
                toggled: !prev.toggled,
            }
        });
    }
}
