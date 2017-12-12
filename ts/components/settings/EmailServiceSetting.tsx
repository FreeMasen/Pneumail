import * as React from 'react';
import TextBox from '../textBox/TextBox';

interface IEmailServiceSettingState {
    isError: boolean;
}

interface IEmailServiceSettingProps {
    service: IEmailService;
    error?: (msg: string) => void;
    update?: (newService: IEmailService) => void;
}

export default class EmailServiceSetting extends React.Component<IEmailServiceSettingProps, IEmailServiceSettingState> {
    private address: string;
    private port: string;
    private username: string;
    private password: string;
    private confirm: string;

    constructor(props) {
        super(props);
        this.state = {
            isError: false,
        }
    }
    render() {
        return (
        <div className={`${this.state.isError ? 'error ' : ''}email-service-container`}>
            <div className="email-service-input-container">
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-address")`} className="input-label">Address</label>
                    <TextBox
                        initialValue={this.props.service.address}
                        onChange={value => this.address = value }
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-port")`} className="input-label">Port</label>
                    <TextBox
                        initialValue={this.props.service.port.toString()}
                        onChange={value => this.port = value}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-username")`} className="input-label">Username</label>
                    <TextBox
                        initialValue={this.props.service.username}
                        onChange={value => this.username = value}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-password")`} className="input-label">Password</label>
                    <TextBox
                        initialValue=""
                        onChange={value => this.password = value}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div>
                    <label htmlFor={`${this.props.service.id}-confirm-password`} className="input-label">Confirm Password</label>
                    <TextBox
                        initialValue=""
                        onChange={value => this.confirm = value}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
            </div>
            <button onClick={ev => this.saveService(ev)} className="form-button">Save</button>
        </div>
        );
    }

    saveService = (ev: React.MouseEvent<HTMLButtonElement>) => {
        console.log('save', ev.currentTarget);
        let service = {} as IEmailService;
        service.id = this.props.service.id;
        service.address = this.address;
        try {
            service.port = parseInt(this.port);

        } catch (e) {
            this.updateError(true);
            return this.props.error(e.message);
        }
        service.username = this.username;
        if (this.password !== this.confirm) {
            this.updateError(true);
            return this.props.error('Passwords must match');
        }
        service.password = this.password;
        console.log('sending update to parent', service);
        this.props.update(service);
        this.updateError(false);
    }

    updateError = (isError: boolean) => {
        return this.setState((prev, props) => {
            return {
                isError
            }
        });
    }
}
