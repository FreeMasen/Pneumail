import * as React from 'react';
import EmailServiceSetting from './EmailServiceSetting';
import Rule from './Rule';

interface ISettingsState {
    errorMsg?: string;
    pendingNewService?: IEmailService
}

interface ISettingsProps {
    EmailServices?: Array<IEmailService>;
    Rules?: Array<any>;
    updateService: (service: IEmailService) => void;

}

export default class Settings extends React.Component<ISettingsProps, ISettingsState> {

    constructor(props) {
        super(props);
        this.state = {
            errorMsg: null,
        }
    }
    render() {
        return (
            <div className="settings">
            {this.state.errorMsg != null &&
            this.state.errorMsg != '' ?
                <span className="error-message">{this.state.errorMsg}</span>
            : null}
                <div className="emails-services-container">
                <h2>Email Services</h2>
                    <div className="email-services paper">
                        {
                            this.props.EmailServices.map(service =>
                                <EmailServiceSetting
                                    service={service}
                                    update={service => this.props.updateService(service)}
                                    error={msg => this.showError(msg) }
                                />)
                        }
                        {
                            this.state.pendingNewService != null ?
                            <EmailServiceSetting
                                service={this.state.pendingNewService}
                                update={service => this.props.updateService(service)}
                                error={msg => this.showError(msg) }
                            />
                            : null
                        }
                        <button
                            className="add service"
                            onClick={ev => this.addService()}
                        >+</button>
                    </div>
                </div>
                <div className="rules-container">
                    <h2>Rules</h2>
                    <div className="rules paper">
                        {
                            this.props.Rules.map(rule =>
                                <Rule
                                    rule={rule}
                                />)
                        }
                        <div
                            className=""
                            onClick={ev => {}}
                        >+</div>
                    </div>
                </div>
            </div>
        );
    }

    showError = (msg: string) => {
        this.setState((prev, props) => {
            return {
                errorMsg: msg,
            }
        });
    }

    addService = () => {
        console.log('Settings.addService')
        this.setState((prev, props) => {
            return {
                pendingNewService: {
                    address: '',
                    port: 0,
                    username: '',
                    password: ''
                }
            };
        });
    }
}
