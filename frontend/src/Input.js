
import styled from "styled-components";

const Input = styled.input`
 padding: 0.5em;
  margin: 0.5em;
  background: papayawhip;
  border: none;
  outline: none;
  border-radius: 3px;
  top: 20%;
  left: 60%;
  position: absolute;
`
const Button = styled.button`
    display: inline-block;
    float: left;
    position: relative;

`

const MessageInput = () => {
    return(
    <div>
    <Button className="button" >Send</Button>
    <Input defaultValue="start chatting" type="text" inputColor="rebeccapurple" />
  </div>
    )
}


export default MessageInput