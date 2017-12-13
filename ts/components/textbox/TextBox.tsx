import * as React from 'react';
import {IconPosition} from '../../enums';

import Logger from '../../logger';

interface ITextBoxState {
    value: string;
    textBoxFocused: boolean;
}

interface ITextBoxProps {
    icon?: JSX.Element;
    iconPosition?: IconPosition;
    initialValue?: string;
    isPassword?: boolean;
    accentColor?: string;
    textColor?: string;
    //dispatch
    onChange: (value) => void;
}

export default class TextBox extends React.Component<ITextBoxProps, ITextBoxState> {
    private waiting = false;
    constructor(props) {
        super(props);
        this.state = {
            value: this.props.initialValue ||'',
            textBoxFocused: false,
        }
    }

    render() {
        return (
            <div className="text-box"
                style={{display: 'flex', flexFlow: 'column', placeContent: 'center'}}>
                <div className="text-box-inner"
                    style={{display: 'flex', flexFlow: 'row', placeContent: 'center'}}>
                    {this.props.iconPosition == IconPosition.Left &&
                    this.props.icon != null ? this.props.icon : (null)}
                    <input
                        type={this.props.isPassword ? 'password' : 'text'}
                        onChange={(event) => this.updateTimer(event)}
                        style={{
                            border: 'none',
                            fontSize: '14pt',
                            background: 'transparent',
                            color: this.props.textColor || 'rgb(255,255,255)',
                            maxWidth: '100%',
                        }}
                        onFocus={(event) => this.toggleFocus(true)}
                        onBlur={(event) => this.toggleFocus(false)}
                        value={this.state.value}
                    />
                    {(!this.props.iconPosition ||
                    this.props.iconPosition == IconPosition.Right) &&
                    this.props.icon != null ? this.props.icon : (null)}
                </div>
                <hr className="text-box-bottom active"
                    style={{
                        margin: '2px 0 0 0',
                        borderTop: '1px solid',
                        borderLeft: 'none',
                        borderRight: 'none',
                        borderBottom: 'none',
                        borderColor: this.props.accentColor || '#fff',
                        width: '100%',
                        transform: `scale(${this.state.textBoxFocused ? 1 : 0})`,
                        transition: 'all 300ms linear',
                        transformOrigin: 'right',
                    }}
                />
            </div>
        );
    }

    updateValue(value: string) {
        this.setState((prev, props) => {
            return {
                value: value
            };
        });
    }

    updateParent(): void {
        this.waiting = false;
        this.props.onChange(this.state.value)
    }

    updateTimer(event): void {
        let value = (event.currentTarget as HTMLInputElement).value;
        this.updateValue(value);
        if (!this.waiting) {
            this.waiting = true;
            setTimeout(() => this.updateParent(), 500)
        }
    }

    toggleFocus(focused: boolean) {
        this.setState((prev, props) => {
            return {
                textBoxFocused: focused
            }
        })
    }
}
