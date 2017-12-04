import * as React from 'react';
import Message from './Message';
interface IMessagesState {

}

interface IMessagesProps {
    title: string;
    messages: Array<any>;
    expanded: boolean;
}

export default class Messages extends React.Component<IMessagesProps, IMessagesState> {

    render() {
        let className = this.props.expanded ? 'messages expanded' : 'messages';
        return (
            <div className={className}>
                <div className="messages-title-container">
                    <span>{this.props.title}</span>
                </div>
                <div className="messages-container paper">
                    {(this.props.messages || []).map(msg => {
                        return (
                            <Message
                                key={msg.id}
                                sender={msg.sender}
                                subject={msg.subject}
                                content={msg.content}
                            />
                        )
                        })
                    }
                </div>
            </div>
            )
    }
}
