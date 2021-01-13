
import styled from "styled-components";

const MessageInput = () => {
    return(
    <div>
      <Input defaultValue="start chatting" type="text" size="49"/>
      <Button className="button" type="submit" >Send</Button>
    </div>
    )
}

const Input = styled.input`
  font-family: Helvetica, sans-serif;
  font-size: small;
  font-color: #454545;

  background: #f2ffe5;
  display: inline-block;
  padding: 0.5em;
  margin: 0.5em;
  border: 2px solid #07333a;
  border-radius: 20px;
  outline: none;
  position: relative;
  left: 50%;
  margin-left: -210px;
`
const Button = styled.button`
    font-family: Helvetica, sans-serif;
    font-size: small;
    font-color: #454545;

    background: #f2ffe5;
    display: inline-block;
    position: relative;
    padding: 0.5em;
    margin: 0.5em;
    border: 2px solid #07333a;
    border-radius: 20px;
    outline: none;
    left: 50%;
    margin-left: -5px;
    
    transition-duration: 0.1s;
    :hover {
      background-color: #83999c;
    }
    :active {
      background-color: #517075;
    }
`

export default MessageInput