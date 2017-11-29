import * as React from 'react';

interface IMessageState {

}

interface IMessageProps extends React.Props<HTMLDivElement> {
    subject?: string;
    sender?: string;
    recipients?: string[];
    coppied?: string[];
    blindCoppied?: string[];
}

export default class Message extends React.Component<IMessageProps, IMessageState> {

    render() {
        return (
            <div className="message">
                <div className="subject-line">
                    <span>{this.props.sender}</span>
                    <span>{this.props.subject}</span>
                </div>
            </div>
        );
    }
}
