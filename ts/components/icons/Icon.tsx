import * as React from 'react';

interface IIconProps {
    className: string;
    width?: number;
    height?: number;
}
export default class Icon extends React.Component<IIconProps, any>{
    render() {
        return (
            <svg
                className={this.props.className}
                width={this.props.width || '100'}
                height={this.props.height || '100'}
                viewBox="0 0 100 100"
                xmlns="http://www.w3.org/2000/svg"
            >
            {this.props.children}
            </svg>
        )
    }
}