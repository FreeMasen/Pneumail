import * as React from 'react';

interface ITopBarProps {
    bannerText: string;

    //dispatch
}

export default class TopBar extends React.Component<ITopBarProps, any> {
    render() {
        return (
            <div className="top-bar paper">
                <span>
                    {this.props.bannerText}
                </span>
            </div>
        )
    }
}